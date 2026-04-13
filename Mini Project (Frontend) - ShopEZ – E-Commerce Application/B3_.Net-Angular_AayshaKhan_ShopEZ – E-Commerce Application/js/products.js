/**
 * products.js
 * Product Module — load JSON, render cards, filter/sort
 *
 * Functions:
 *   loadProducts(callback)             — fetch products.json, populate allProducts + productMap
 *   createProductCard(product, cols)   — build Bootstrap card HTML for one product
 *   displayProducts(products, id)      — render product array into a grid container
 *   applyFilters()                     — read all filter inputs and re-render matching products
 *   clearFilters()                     — reset all filters, clear URL params, show all products
 */

/* Module-level cache — populated once by loadProducts() */
let allProducts = [];

/*
 * productMap: { [id]: product }
 * Stores products keyed by id so onclick handlers can pass only a safe
 * integer instead of inlining the full JSON object (which breaks on
 * product names containing quotes and poses an XSS risk).
 */
const productMap = {};

/* ─── Column class map ──────────────────────────────────────────────
 * Different grids need different column widths:
 *   home page (featuredGrid, bestSellerGrid) → 4 cards per row on desktop
 *   products page (productsGrid)             → 3 cards per row on desktop
 * Passing the containerId lets createProductCard() pick the right classes
 * without any CSS overrides that would fight Bootstrap's grid system.
 */
const GRID_COL_CLASSES = {
  featuredGrid:   'col-xl-3 col-lg-3 col-md-6 col-sm-6',
  bestSellerGrid: 'col-xl-3 col-lg-3 col-md-6 col-sm-6',
  productsGrid:   'col-lg-4 col-md-6 col-sm-6',
};

/* Default fallback if an unknown containerId is passed */
const DEFAULT_COL = 'col-lg-4 col-md-6 col-sm-6';

/* ─── Load Products ─────────────────────────────────────────────────
 * Fetches data/products.json via jQuery AJAX.
 * On success: caches products in allProducts and productMap, then calls
 *             the optional callback with the full product array.
 * On failure: shows an error toast AND replaces any skeleton/grid
 *             containers with a visible error state so users are never
 *             left staring at an infinite spinner.
 */
const loadProducts = (callback) => {
  $.getJSON('data/products.json')
    .done(data => {
      allProducts = data;
      data.forEach(p => { productMap[p.id] = p; });
      if (typeof callback === 'function') callback(data);
    })
    .fail(() => {
      showToast('Failed to load products. Please refresh.', 'error');

      /*
       * Replace skeleton loaders with a proper error state.
       * We check every known grid so this works on both index.html
       * (featuredGrid / bestSellerGrid / categoriesGrid) and
       * products.html (productsGrid).
       */
      const errorHtml = `
        <div class="empty-state">
          <div class="empty-state-icon">
            <ion-icon name="cloud-offline-outline"></ion-icon>
          </div>
          <h4>Could Not Load Products</h4>
          <p>Something went wrong while fetching product data.</p>
          <button class="btn btn-primary" onclick="location.reload()">
            <ion-icon name="refresh-outline"></ion-icon> Retry
          </button>
        </div>`;

      ['productsGrid', 'featuredGrid', 'bestSellerGrid', 'categoriesGrid'].forEach(id => {
        const el = document.getElementById(id);
        if (!el) return;
        /* Remove row class so the block-level empty-state centers correctly */
        el.classList.remove('row');
        /* Only show the error state in grids that were showing skeletons */
        if (id !== 'categoriesGrid') el.innerHTML = errorHtml;
        else el.innerHTML = '';
      });
    });
};

/* ─── Build Product Card ────────────────────────────────────────────
 * Returns a Bootstrap column + card HTML string for one product.
 *
 * @param {Object} product     — product data object from products.json
 * @param {string} containerId — the grid this card will be placed in;
 *                               used to select the correct column classes
 *                               so home page shows 4 per row and
 *                               products page shows 3 per row.
 */
const createProductCard = (product, containerId = 'productsGrid') => {
  const { id, name, price, originalPrice, image, category, badge, rating, reviews } = product;

  const colClass  = GRID_COL_CLASSES[containerId] || DEFAULT_COL;
  const badgeHtml = badge ? `<span class="product-discount-badge">${badge}</span>` : '';
  const origHtml  = originalPrice
    ? `<span class="product-price-original">${formatPrice(originalPrice)}</span>`
    : '';

  return `
    <div class="${colClass} mb-4">
      <div class="card product-card">
        <div class="product-img-wrap">
          ${badgeHtml}
          <img
            class="card-img-top"
            src="${image}"
            alt="${name}"
            onerror="this.src='https://placehold.co/220x190/e2e8f0/94a3b8?text=No+Image'"
            loading="lazy"
          >
        </div>
        <div class="card-body">
          <p class="product-category-label">${category}</p>
          <h6 class="card-title">${name}</h6>
          <div class="product-stars">
            <span class="star-icons">${renderStars(rating)}</span>
            <span class="product-review-count">(${reviews.toLocaleString()})</span>
          </div>
          <div>
            <span class="product-price-current">${formatPrice(price)}</span>
            ${origHtml}
          </div>
        </div>
        <div class="card-footer">
          <button
            class="btn btn-primary btn-sm flex-grow-1"
            onclick="addToCart(productMap[${id}])"
          >
            <ion-icon name="cart-outline"></ion-icon> Add to Cart
          </button>
          <a href="product-details.html?id=${id}" class="btn btn-light btn-sm">
            <ion-icon name="eye-outline"></ion-icon> View
          </a>
        </div>
      </div>
    </div>`;
};

/* ─── Render Grid ───────────────────────────────────────────────────
 * Renders an array of products into the specified grid container.
 *
 * Key behaviour: the grid element in HTML has class="row" which makes
 * it a flex container (justify-content: flex-start). Injecting a col-12
 * wrapper for the empty state creates a flex item that gets left-aligned
 * regardless of text-center or mx-auto. Instead:
 *   • remove "row" when showing empty state → grid becomes a plain block
 *     container → empty-state fills full width → its own flex +
 *     align-items:center centers all children correctly.
 *   • restore "row" when showing product cards so Bootstrap grid works.
 *
 * @param {Array}  products    — filtered/sorted product array to display
 * @param {string} containerId — id of the target grid element
 */
const displayProducts = (products, containerId = 'productsGrid') => {
  const grid = document.getElementById(containerId);
  if (!grid) return;

  if (!products.length) {
    /* Switch to block layout so empty-state fills width and centers */
    grid.classList.remove('row');
    grid.innerHTML = `
      <div class="empty-state">
        <div class="empty-state-icon">
          <ion-icon name="search-outline"></ion-icon>
        </div>
        <h4>No Products Found</h4>
        <p>Try adjusting your filters or search term.</p>
        <button class="btn btn-primary" onclick="clearFilters()">
          <ion-icon name="refresh-outline"></ion-icon> Clear Filters
        </button>
      </div>`;
    return;
  }

  /* Restore row class so Bootstrap grid layout works for product cards */
  grid.classList.add('row');
  grid.innerHTML = products.map(p => createProductCard(p, containerId)).join('');

  /* Update the "Showing N products" counter if present on this page */
  const countEl = document.getElementById('productCount');
  if (countEl) countEl.textContent = products.length;
};

/* ─── Apply Filters ─────────────────────────────────────────────────
 * Reads all filter/search/sort inputs, chains the filters over
 * allProducts, sorts the result, and calls displayProducts().
 *
 * Two separate search bar IDs (searchBarDesktop / searchBarMobile) exist
 * because Bootstrap hides one based on breakpoint — using a single ID
 * would cause getElementById() to return only the first match, making
 * whichever is hidden silently override the visible one.
 */
const applyFilters = () => {
  const selectedCats = [...document.querySelectorAll('.filter-cat:checked')].map(c => c.value);

  /*
   * Explicit empty-string check instead of || 0 / || Infinity:
   * a falsy check treats user-entered 0 as "not set", so entering 0 as
   * max price would incorrectly become Infinity. Explicit check makes
   * 0 a valid input for both min and max price fields.
   */
  const minPriceEl = document.getElementById('minPrice');
  const maxPriceEl = document.getElementById('maxPrice');
  const minPrice   = (minPriceEl && minPriceEl.value !== '') ? parseFloat(minPriceEl.value) : 0;
  const maxPrice   = (maxPriceEl && maxPriceEl.value !== '') ? parseFloat(maxPriceEl.value) : Infinity;

  const minRating = parseFloat(document.getElementById('filterRating')?.value) || 0;
  const sortBy    = document.getElementById('sortSelect')?.value || 'default';

  /* Read whichever search bar is currently visible */
  const desktopVal = document.getElementById('searchBarDesktop')?.value || '';
  const mobileVal  = document.getElementById('searchBarMobile')?.value  || '';
  const searchTerm = (desktopVal || mobileVal).toLowerCase().trim();

  let results = [...allProducts];

  if (selectedCats.length)
    results = results.filter(p => selectedCats.includes(p.category));

  results = results.filter(p => p.price >= minPrice && p.price <= maxPrice);

  if (minRating)
    results = results.filter(p => p.rating >= minRating);

  if (searchTerm)
    results = results.filter(p =>
      p.name.toLowerCase().includes(searchTerm)        ||
      p.category.toLowerCase().includes(searchTerm)    ||
      p.description.toLowerCase().includes(searchTerm)
    );

  const sorters = {
    'price-asc':  (a, b) => a.price - b.price,
    'price-desc': (a, b) => b.price - a.price,
    'rating':     (a, b) => b.rating - a.rating,
    'popular':    (a, b) => b.reviews - a.reviews,
    'name':       (a, b) => a.name.localeCompare(b.name),
  };

  if (sorters[sortBy]) results.sort(sorters[sortBy]);

  displayProducts(results);
};

/* ─── Clear Filters ─────────────────────────────────────────────────
 * Resets all filter controls to their default state and shows all
 * products. Also strips any ?search= or ?category= query string from
 * the URL using history.replaceState — this prevents the old search
 * term from being re-applied if the user refreshes the page, without
 * triggering a page reload.
 */
const clearFilters = () => {
  /* Remove ?search= / ?category= from address bar without page reload */
  if (window.location.search) {
    history.replaceState(null, '', window.location.pathname);
  }

  document.querySelectorAll('.filter-cat').forEach(cb => (cb.checked = false));

  ['minPrice', 'maxPrice', 'sortSelect', 'searchBarDesktop', 'searchBarMobile', 'filterRating']
    .forEach(id => {
      const el = document.getElementById(id);
      if (el) el.value = '';
    });

  displayProducts(allProducts);
};
