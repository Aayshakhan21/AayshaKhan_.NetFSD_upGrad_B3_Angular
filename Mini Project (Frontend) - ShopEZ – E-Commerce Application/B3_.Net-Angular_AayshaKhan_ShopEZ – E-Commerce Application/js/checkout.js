/**
 * checkout.js
 * Checkout Module — order summary, form validation, order simulation
 */

$(document).ready(() => {
  const cart = getCart();
  if (!cart.length) {
    window.location.href = 'cart.html';
    return;
  }
  buildOrderSummary();
});

/* ── Build Order Summary  */

const buildOrderSummary = () => {
  const cart      = getCart();
  const container = document.getElementById('orderItems');
  if (!container) return;

  container.innerHTML = cart.map(({ name, image, price, qty }) => {
    const quantity = qty || 1;
    return `
      <div class="co-order-item">
        <img
          class="co-order-img"
          src="${image}"
          alt="${name}"
          onerror="this.src='https://placehold.co/52x52/e2e8f0/94a3b8?text=No+Image'"
        >
        <div class="co-order-name">
          ${name}
          <div class="text-muted" style="font-size:.75rem;font-weight:400">Qty: ${quantity}</div>
        </div>
        <div class="co-order-price">${formatPrice(price * quantity)}</div>
      </div>`;
  }).join('');

  const total    = getCartTotal();
  const savings  = getOrigTotal() - total;
  const shipping = getShipping();

  const set   = (id, val) => { const el = document.getElementById(id); if (el) el.textContent = val; };
  const count = cart.reduce((s, i) => s + (i.qty || 1), 0);

  set('coCount',    count);
  set('coSubtotal', formatPrice(total));
  set('coSavings',  formatPrice(savings));
  set('coShipping', shipping === 0 ? 'FREE' : formatPrice(shipping));
  set('coTotal',    formatPrice(total + shipping));

 // Hide the savings row when there are no discounted items in the cart
  const savingsRow = document.getElementById('coSavingsRow');
  if (savingsRow) savingsRow.style.display = savings > 0 ? '' : 'none';
};

/* ── Form Validation  */

const FIELD_RULES = [
  { id: 'coName',    errId: 'errName',    label: 'Full Name',    minLen: 3 },
  { id: 'coEmail',   errId: 'errEmail',   label: 'Email',        type: 'email' },
  { id: 'coPhone',   errId: 'errPhone',   label: 'Phone',        type: 'phone' },
  { id: 'coAddress', errId: 'errAddress', label: 'Address',      minLen: 10 },
  { id: 'coCity',    errId: 'errCity',    label: 'City',         minLen: 2 },
  { id: 'coState',   errId: 'errState',   label: 'State',        type: 'select' },
  { id: 'coPincode', errId: 'errPincode', label: 'Pincode',      type: 'pincode' },
];

const VALIDATORS = {
  email:   (v) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v),
  phone:   (v) => /^[6-9]\d{9}$/.test(v),
  pincode: (v) => /^\d{6}$/.test(v),
  select:  (v) => v !== '' && v !== null,
};

const ERROR_MSGS = {
  email:   'Enter a valid email address.',
  phone:   'Enter a valid 10-digit phone number.',
  pincode: 'Enter a valid 6-digit pincode.',
  select:  'Please select an option.',
};

const validateForm = () => {
  let valid = true;

  FIELD_RULES.forEach(({ id, errId, label, type, minLen }) => {
    const input = document.getElementById(id);
    const errEl = document.getElementById(errId);
    if (!input) return;

    const val = input.value.trim();
    let error = '';

    if (!val) {
      error = `${label} is required.`;
    } else if (minLen && val.length < minLen) {
      error = `${label} must be at least ${minLen} characters.`;
    } else if (type && VALIDATORS[type] && !VALIDATORS[type](val)) {
      error = ERROR_MSGS[type];
    }

    if (error) {
      input.classList.add('is-invalid');
      if (errEl) { errEl.textContent = error; errEl.style.display = 'block'; }
      valid = false;
    } else {
      input.classList.remove('is-invalid');
      if (errEl) errEl.style.display = 'none';
    }
  });

  return valid;
};

/* ── Place Order  */

const placeOrder = () => {
  if (!validateForm()) {
    showToast('Please fill in all required fields correctly.', 'error');
    return;
  }

  const btn = document.getElementById('placeOrderBtn');
  if (btn) {
    btn.disabled  = true;
    btn.innerHTML = `<ion-icon name="hourglass-outline"></ion-icon> Processing…`;
  }

  setTimeout(() => {
    const orderId = `SHEZ${Date.now().toString().slice(-8)}`;
    const name    = document.getElementById('coName').value;
    const total   = getCartTotal() + getShipping();

    // Read the selected payment method and show it in the success modal
    const paymentLabels = { cod: 'Cash on Delivery', upi: 'UPI / QR Code', card: 'Credit / Debit Card' };
    const selectedPayment = document.querySelector('input[name="paymentMethod"]:checked')?.value || 'cod';

    const set = (id, val) => { const el = document.getElementById(id); if (el) el.textContent = val; };
    set('modalOrderId',      orderId);
    set('modalOrderName',    name);
    set('modalOrderTotal',   formatPrice(total));
    set('modalOrderPayment', paymentLabels[selectedPayment] || selectedPayment);

    const modal = new bootstrap.Modal(document.getElementById('successModal'));
    document.getElementById('successModal').addEventListener('hidden.bs.modal', () => {
      if (btn) {
        btn.disabled  = false;
        btn.innerHTML = `<ion-icon name="checkmark-circle-outline"></ion-icon> Place Order`;
      }
    }, { once: true });

    modal.show();

    saveCart([]);
  }, 1600);
};

/* ── Live validation reset on input  */

$(document).on('input change', '.form-control, .form-select', function () {
  $(this).removeClass('is-invalid');
  $(this).siblings('.invalid-feedback').hide();
});
