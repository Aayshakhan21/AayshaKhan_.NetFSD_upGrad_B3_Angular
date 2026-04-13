# 🛍️ ShopEZ — Frontend E-Commerce Application

> A fully functional, client-side e-commerce web application built with HTML5, CSS3, JavaScript ES6, Bootstrap 5, and jQuery.

---

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Live Demo & Navigation](#live-demo--navigation)
- [Tech Stack](#tech-stack)
- [Folder Structure](#folder-structure)
- [Pages](#pages)
- [Features](#features)
- [JavaScript Modules](#javascript-modules)
- [Product Data (JSON)](#product-data-json)
- [LocalStorage — Cart Persistence](#localstorage--cart-persistence)
- [How to Run](#how-to-run)
- [Known Limitations](#known-limitations)

---

## Project Overview

ShopEZ is a **frontend-only** prototype of an e-commerce store for premium electronics and gadgets. It demonstrates core frontend development concepts — responsive layouts, dynamic DOM manipulation, client-side state management, and form validation — without any backend server.

All product data is stored in a JSON file. Cart data is persisted using the browser's `localStorage`. No database, no server, no build tools needed.

---

## Live Demo & Navigation

Open `index.html` in any modern browser. No installation required.

```
Home Page  →  Products Page  →  Product Details  →  Add to Cart  →  Cart Page  →  Checkout
```

---

## Tech Stack

| Technology | Version | Purpose |
|---|---|---|
| HTML5 | — | Page structure and semantic markup |
| CSS3 | — | Custom styling, animations, responsive layout |
| JavaScript | ES6+ | Application logic, DOM manipulation |
| Bootstrap | 5.3.2 | Responsive grid, UI components |
| jQuery | 3.7.1 | DOM utilities, AJAX ($.getJSON), events |
| LocalStorage | Browser API | Cart data persistence across pages |
| JSON | — | Product data source (`data/products.json`) |

---

## Folder Structure

```
ShopEZ/
│
├── index.html               ← Home page
├── products.html            ← Full product catalog
├── product-details.html     ← Single product view
├── cart.html                ← Shopping cart
├── checkout.html            ← Order form + simulation
│
├── css/
│   └── styles.css           ← All custom styles (1252 lines)
│
├── js/
│   ├── common.js            ← Shared utilities (cart, toast, format)
│   ├── products.js          ← Product loading, cards, filters
│   ├── cart.js              ← Cart rendering and management
│   └── checkout.js          ← Form validation and order simulation
│
├── data/
│   └── products.json        ← 12 products with full metadata
│
├── images/
│   ├── laptop.jpeg
│   ├── smartphone.jpeg
│   └── ... (12 product images)
│
└── lib/
    ├── bootstrap/           ← (Place Bootstrap files here for offline use)
    └── jquery/              ← (Place jQuery files here for offline use)
```

---

## Pages

### 1. `index.html` — Home Page
- Announcement bar with promo code
- Responsive navbar with search (desktop + mobile offcanvas)
- Hero banner with CTA buttons
- Feature strip (Free Delivery, Secure Payments, Easy Returns, Support)
- Dynamic category cards built from product data
- Featured Products (top 4 by rating)
- Best Sellers (next 4 by review count)
- Newsletter subscription form
- Footer with quick links, categories, social icons

### 2. `products.html` — Products Page
- Full product catalog with Bootstrap card grid
- Filter sidebar: Category (checkboxes), Price Range (min/max), Min Rating
- Sort options: Relevance, Price (Low–High / High–Low), Highest Rated, Most Popular, Name A–Z
- Search bar (separate IDs for mobile and desktop to avoid DOM conflicts)
- Product count display
- URL parameter support: `?category=Electronics`, `?search=laptop`
- Empty state with Clear Filters button

### 3. `product-details.html` — Product Detail Page
- Loading skeleton while data fetches
- Product not found state (invalid `?id=` param)
- Large product image with error fallback
- Name, category badge, discount badge, rating stars, review count
- Current price, original price, savings tag
- Add to Cart and Buy Now buttons
- Stock status, warranty, return policy, delivery info
- Tab panel: Description / Specifications table / Reviews
- Related products grid (same category, different id)

### 4. `cart.html` — Shopping Cart
- Empty cart state with link back to products
- Cart item rows with image, name, category, price, quantity controls (+/-)
- Per-item subtotal updating with qty
- Remove item button with confirmation toast
- Clear entire cart button with confirm dialog
- Order Summary card: Original Price, Savings (hidden when zero), Subtotal, Delivery (FREE above ₹50,000), Total
- Proceed to Checkout and Continue Shopping buttons

### 5. `checkout.html` — Checkout Page
- Redirect to cart if cart is empty
- Step 1 — Delivery Information form:
  - Full Name (min 3 chars)
  - Email (regex validated)
  - Phone (Indian 10-digit, starts 6–9)
  - Street Address (min 10 chars)
  - City (min 2 chars)
  - State (required select — empty default validated)
  - Pincode (exactly 6 digits)
- Step 2 — Payment Method (UI only, simulated)
- Right panel — Live Order Summary with all cart items, totals, shipping
- Place Order button → 1.6s processing state → Success modal with unique Order ID → cart cleared

## JavaScript Modules

### `common.js` — Shared Utilities
All cart operations and helper functions. Must be loaded **before** other JS files.

```js
getCart()             // Read cart from localStorage → Array
saveCart(cart)        // Write cart to localStorage + update badge
addToCart(product)    // Add item or increment qty if already in cart
removeFromCart(id)    // Remove item by product id
updateCartQty(id, ±1) // Increment or decrement qty (min: 1)
getCartCount()        // Total item count (sum of all qty)
getCartTotal()        // Total price (sum of price × qty)
getOrigTotal()        // Total at original prices (for savings calc)
getShipping()         // 0 if total >= ₹50,000 else ₹99
formatPrice(n)        // "₹89,999" — Indian locale formatting
renderStars(rating)   // "★★★★✩" — 5-point star string
showToast(msg, type)  // Floating toast: 'success' | 'error' | 'info'
getURLParam(key)      // Read query string param from current URL
updateCartBadge()     // Update navbar cart count badge
```

### `products.js` — Product Module
Loaded on: `index.html`, `products.html`, `product-details.html`

```js
loadProducts(callback)          // Fetch data/products.json → populate allProducts + productMap
createProductCard(product)      // Build Bootstrap card HTML string for one product
displayProducts(products, id?)  // Render array of products into a grid container
applyFilters()                  // Chain category + price + rating + search + sort filters
clearFilters()                  // Reset all filter inputs → show all products
```

**Key variables:**
- `allProducts` — full product array from JSON
- `productMap` — `{ [id]: product }` lookup for safe onclick references

### `cart.js` — Cart Module
Loaded on: `cart.html`

```js
renderCartPage()       // Show empty state or render cart items + summary
buildCartRow(item)     // Build HTML for one cart item row
changeQty(id, delta)   // Update qty via updateCartQty() then re-render
removeItem(id)         // Remove item, show toast, re-render
clearEntireCart()      // Confirm dialog → saveCart([]) → re-render
renderSummary()        // Compute and display all summary totals
```

### `checkout.js` — Checkout Module
Loaded on: `checkout.html`

```js
buildOrderSummary()   // Render all cart items and totals in right panel
validateForm()        // Validate all FIELD_RULES → mark is-invalid + show errors
placeOrder()          // Validate → processing state → success modal → clear cart
```

**Validation rules:**

| Field | Rule |
|---|---|
| Full Name | Required, min 3 characters |
| Email | Required, regex `/^[^\s@]+@[^\s@]+\.[^\s@]+$/` |
| Phone | Required, regex `/^[6-9]\d{9}$/` (Indian mobile) |
| Address | Required, min 10 characters |
| City | Required, min 2 characters |
| State | Required, select must not be empty default |
| Pincode | Required, regex `/^\d{6}$/` |

---

## Product Data (JSON)

File: `data/products.json`

```json
{
  "id":           1,
  "name":         "Laptop Pro X15",
  "description":  "High-performance laptop...",
  "price":        89999,
  "originalPrice": 99999,
  "image":        "images/laptop.jpeg",
  "category":     "Electronics",
  "badge":        "-10%",
  "rating":       4.8,
  "reviews":      2341
}
```

| Field | Type | Required | Description |
|---|---|---|---|
| `id` | Number | ✅ | Unique product identifier |
| `name` | String | ✅ | Product display name |
| `description` | String | ✅ | Full product description |
| `price` | Number | ✅ | Current selling price in ₹ |
| `image` | String | ✅ | Relative path to image file |
| `originalPrice` | Number / null | ➕ Bonus | MRP before discount (null = no discount) |
| `category` | String | ➕ Bonus | Used for filtering — Electronics, Audio, etc. |
| `badge` | String / null | ➕ Bonus | Discount label e.g. `"-10%"` |
| `rating` | Number | ➕ Bonus | 0–5 star rating |
| `reviews` | Number | ➕ Bonus | Total review count |

**To add a new product** — append an object to `data/products.json` and add the image to `images/`. The UI rebuilds automatically.

---

## LocalStorage — Cart Persistence

Cart data is stored under the key `shopez_cart` as a JSON string.

**Structure stored:**
```json
[
  { "id": 1, "name": "Laptop Pro X15", "price": 89999, "qty": 2, "image": "images/laptop.jpeg", ... },
  { "id": 3, "name": "Headphones",     "price": 22999, "qty": 1, "image": "images/headphones.jpeg", ... }
]
```

The cart:
- Persists across all page navigations in the same browser
- Is automatically cleared after a successful order is placed
- Updates the navbar badge count on every page via `updateCartBadge()` in `common.js`
- Can be inspected via DevTools → Application → Local Storage

---

## How to Run

**Option 1 — Open directly (simplest):**
```
Double-click index.html in your file manager
```
> ⚠️ `$.getJSON()` requires a server. Opening via `file://` may block the JSON fetch in some browsers.

**Option 2 — VS Code Live Server (recommended):**
1. Install the **Live Server** extension in VS Code
2. Right-click `index.html` → **Open with Live Server**
3. Browse to `http://127.0.0.1:5500`

**Option 3 — Python HTTP server:**
```bash
cd ShopEZ/
python3 -m http.server 5500
# Open http://localhost:5500 in your browser
```

**Option 4 — Node HTTP server:**
```bash
npx serve .
# Open the URL shown in terminal
```

---

## Known Limitations

| Limitation | Reason |
|---|---|
| No real payment processing | Frontend prototype only |
| No user authentication / login | Out of scope for this project |
| No order history | No backend or database |
| `lib/` folders are empty | Bootstrap and jQuery loaded from CDN — requires internet |
| Product data is static | Adding/removing products requires editing `products.json` manually |
| Reviews are simulated | Same 3 placeholder reviews shown for all products |

