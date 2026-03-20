/**
 * cart.js
 * Cart Module — render cart items, handle qty & removal
 */

$(document).ready(() => renderCartPage());

/*  Render Full Cart Page  */

const renderCartPage = () => {
  const cart      = getCart();
  const emptyEl   = document.getElementById('cartEmpty');
  const contentEl = document.getElementById('cartContent');

  if (!cart.length) {
    if (emptyEl)   emptyEl.style.display   = 'block';
    if (contentEl) contentEl.style.display = 'none';
    return;
  }

  if (emptyEl)   emptyEl.style.display   = 'none';
  if (contentEl) contentEl.style.display = '';

  const container = document.getElementById('cartItems');
  if (container) container.innerHTML = cart.map(buildCartRow).join('');

  renderSummary();
};

/*  Build Cart Item Row HTML  */

const buildCartRow = (item) => {
  const { id, name, price, originalPrice, image, category } = item;
  const qty      = item.qty || 1;
  const subtotal = price * qty;
  const hasDisco = originalPrice && originalPrice > price;

  return `
    <div class="cart-item-row" id="cart-item-${id}">
      <img
        class="cart-item-img"
        src="${image}"
        alt="${name}"
        onerror="this.src='https://placehold.co/90x90/e2e8f0/94a3b8?text=No+Image'"
      >
      <div class="cart-item-body">
        <div class="cart-item-name">${name}</div>
        <div class="cart-item-cat">${category}</div>
        <div>
          <span class="cart-item-price">${formatPrice(subtotal)}</span>
          ${hasDisco ? `<span class="cart-item-orig">${formatPrice(originalPrice * qty)}</span>` : ''}
        </div>
        <div class="qty-controls">
          <!-- FIX: disabled attribute added when qty === 1 so the button dims and
               cannot be clicked, giving the user clear visual feedback that 1 is
               the minimum quantity. The disabled state is re-evaluated each time
               renderCartPage() rebuilds the row from current cart state. -->
          <button class="qty-btn" onclick="changeQty(${id}, -1)" ${qty <= 1 ? 'disabled' : ''}>
            <ion-icon name="remove-outline"></ion-icon>
          </button>
          <span class="qty-value" id="qty-${id}">${qty}</span>
          <button class="qty-btn" onclick="changeQty(${id}, 1)">
            <ion-icon name="add-outline"></ion-icon>
          </button>
          <span class="product-review-count ms-1">× ${formatPrice(price)} each</span>
        </div>
      </div>
      <button class="btn btn-sm btn-outline-danger align-self-start" onclick="removeItem(${id})">
        <ion-icon name="trash-outline"></ion-icon>
        <span class="d-none d-sm-inline">Remove</span>
      </button>
    </div>`;
};

/*  Qty + Remove Actions  */

const changeQty = (productId, delta) => {
  updateCartQty(productId, delta);
  renderCartPage();
};

const removeItem = (productId) => {
  removeFromCart(productId);
  showToast('Item removed from cart.', 'info');
  renderCartPage();
};

const clearEntireCart = () => {
  if (!confirm('Remove all items from your cart?')) return;
  saveCart([]);
  showToast('Cart cleared.', 'info');
  renderCartPage();
};

/*  Summary Render  */

const renderSummary = () => {
  const total    = getCartTotal();
  const origTotal = getOrigTotal();
  const savings  = origTotal - total;
  const shipping = getShipping();
  const finalAmt = total + shipping;
  const count    = getCart().reduce((s, i) => s + (i.qty || 1), 0);

  const set = (id, val) => { const el = document.getElementById(id); if (el) el.textContent = val; };

  set('summaryCount',    count);
  set('summaryOriginal', formatPrice(origTotal));
  set('summarySavings',  formatPrice(savings));
  set('summarySubtotal', formatPrice(total));
  set('summaryShipping', shipping === 0 ? 'FREE' : formatPrice(shipping));
  set('summaryTotal',    formatPrice(finalAmt));

  // Hide the savings row when there are no actual savings.
  const savingsRow = document.getElementById('savingsRow');
  if (savingsRow) savingsRow.style.display = savings > 0 ? '' : 'none';
};
