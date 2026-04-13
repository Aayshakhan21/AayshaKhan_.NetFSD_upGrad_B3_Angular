/**
 * common.js
 * Shared utilities — cart management, toast, formatting
 */

/* ── Cart Helpers  */

const getCart = () => {
  try {
    return JSON.parse(localStorage.getItem('shopez_cart')) || [];
  } catch {
    localStorage.removeItem('shopez_cart');
    return [];
  }
};

const saveCart = (cart) => {
  localStorage.setItem('shopez_cart', JSON.stringify(cart));
  updateCartBadge();
};

const addToCart = (product) => {
  const cart     = getCart();
  const existing = cart.find(item => item.id === product.id);

  if (existing) {
    existing.qty = (existing.qty || 1) + 1;
    showToast(`${product.name} quantity updated!`, 'success');
  } else {
    cart.push({ ...product, qty: 1 });
    showToast(`${product.name} added to cart!`, 'success');
  }
  saveCart(cart);
};

const removeFromCart = (productId) => {
  const cart = getCart().filter(item => item.id !== productId);
  saveCart(cart);
};

const updateCartQty = (productId, delta) => {
  const cart = getCart();
  const item = cart.find(i => i.id === productId);
  if (!item) return;
  item.qty = Math.max(1, (item.qty || 1) + delta);
  saveCart(cart);
};

const getCartCount = () => getCart().reduce((sum, i) => sum + (i.qty || 1), 0);
const getCartTotal = () => getCart().reduce((sum, i) => sum + i.price * (i.qty || 1), 0);
const getOrigTotal = () => getCart().reduce((sum, i) => sum + (i.originalPrice || i.price) * (i.qty || 1), 0);


const getShipping = () => {
  const total = getCartTotal();
  return total > 0 ? (total >= 50000 ? 0 : 99) : 0;
};

const updateCartBadge = () => {
  const badge = document.getElementById('cartCountBadge');
  if (!badge) return;
  const count = getCartCount();
  badge.textContent   = count;
  badge.style.display = count > 0 ? 'flex' : 'none';
};

/* ── Formatting  */

const formatPrice = (n) => `₹${Number(n).toLocaleString('en-IN')}`;

const renderStars = (rating) => {
  const full  = Math.floor(rating);
  const half  = rating % 1 >= 0.5 ? 1 : 0;
  const empty = 5 - full - half;
  return '★'.repeat(full) + (half ? '✩' : '') + '☆'.repeat(empty);
};

/* ── Toast Notifications  */

const showToast = (message, type = 'info') => {
  const icons = { success: 'checkmark-circle', error: 'close-circle', info: 'information-circle' };

  let wrap = document.getElementById('toastContainer');
  if (!wrap) {
    wrap = document.createElement('div');
    wrap.id        = 'toastContainer';
    wrap.className = 'toast-container';
    document.body.appendChild(wrap);
  }

  const toast = document.createElement('div');
  toast.className = `toast-msg ${type}`;
  toast.innerHTML = `<ion-icon name="${icons[type]}"></ion-icon><span>${message}</span>`;
  wrap.appendChild(toast);

  setTimeout(() => {
    toast.classList.add('out');
    setTimeout(() => toast.remove(), 300);
  }, 3000);
};

/* ── URL Params  */

const getURLParam = (key) => new URLSearchParams(window.location.search).get(key);

/* ── Init on DOM Ready  */

$(document).ready(() => updateCartBadge());
