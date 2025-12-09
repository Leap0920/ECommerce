/**
 * ECommerce API Service
 * This file provides JavaScript functions to interact with the backend API
 */

const ECommerceAPI = {
    // Base URL for API calls
    baseUrl: '',

    // ==================== PRODUCTS API ====================
    products: {
        /**
         * Get all products
         */
        getAll: function () {
            return fetch('/api/products')
                .then(response => response.json());
        },

        /**
         * Get product by ID
         * @param {number} id - Product ID
         */
        getById: function (id) {
            return fetch(`/api/products/get/${id}`)
                .then(response => response.json());
        },

        /**
         * Get products by type
         * @param {string} type - Product type (e.g., 'Electronics', 'Clothing')
         */
        getByType: function (type) {
            return fetch(`/api/products/type/${encodeURIComponent(type)}`)
                .then(response => response.json());
        },

        /**
         * Search products
         * @param {string} query - Search query
         */
        search: function (query) {
            return fetch(`/api/products/search?q=${encodeURIComponent(query)}`)
                .then(response => response.json());
        },

        /**
         * Get favorite products
         */
        getFavorites: function () {
            return fetch('/api/products/favorites')
                .then(response => response.json());
        },

        /**
         * Create a new product (Admin only)
         * @param {object} product - Product data
         */
        create: function (product) {
            return fetch('/api/products/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(product)
            }).then(response => response.json());
        },

        /**
         * Update a product (Admin only)
         * @param {object} product - Product data with ID
         */
        update: function (product) {
            return fetch('/api/products/update', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(product)
            }).then(response => response.json());
        },

        /**
         * Delete a product (Admin only)
         * @param {number} id - Product ID
         */
        delete: function (id) {
            return fetch(`/api/products/delete/${id}`, {
                method: 'POST'
            }).then(response => response.json());
        }
    },

    // ==================== CART API ====================
    cart: {
        /**
         * Get current cart
         */
        get: function () {
            return fetch('/api/cart')
                .then(response => response.json());
        },

        /**
         * Add item to cart
         * @param {number} productId - Product ID
         * @param {number} quantity - Quantity (default: 1)
         */
        add: function (productId, quantity = 1) {
            const formData = new FormData();
            formData.append('productId', productId);
            formData.append('quantity', quantity);

            return fetch('/api/cart/add', {
                method: 'POST',
                body: formData
            }).then(response => response.json());
        },

        /**
         * Update cart item quantity
         * @param {number} productId - Product ID
         * @param {number} quantity - New quantity
         */
        update: function (productId, quantity) {
            const formData = new FormData();
            formData.append('productId', productId);
            formData.append('quantity', quantity);

            return fetch('/api/cart/update', {
                method: 'POST',
                body: formData
            }).then(response => response.json());
        },

        /**
         * Remove item from cart
         * @param {number} productId - Product ID
         */
        remove: function (productId) {
            const formData = new FormData();
            formData.append('productId', productId);

            return fetch('/api/cart/remove', {
                method: 'POST',
                body: formData
            }).then(response => response.json());
        },

        /**
         * Clear the entire cart
         */
        clear: function () {
            return fetch('/api/cart/clear', {
                method: 'POST'
            }).then(response => response.json());
        },

        /**
         * Get cart item count
         */
        getCount: function () {
            return fetch('/api/cart/count')
                .then(response => response.json());
        }
    },

    // ==================== ORDERS API ====================
    orders: {
        /**
         * Get all orders (Admin only)
         */
        getAll: function () {
            return fetch('/api/orders')
                .then(response => response.json());
        },

        /**
         * Get order by ID
         * @param {string} id - Order ID
         */
        getById: function (id) {
            return fetch(`/api/orders/get/${encodeURIComponent(id)}`)
                .then(response => response.json());
        },

        /**
         * Get current user's orders
         */
        getUserOrders: function () {
            return fetch('/api/orders/user')
                .then(response => response.json());
        },

        /**
         * Get recent orders
         * @param {number} count - Number of orders to fetch
         */
        getRecent: function (count = 10) {
            return fetch(`/api/orders/recent/${count}`)
                .then(response => response.json());
        },

        /**
         * Place an order (checkout)
         * @param {object} checkoutData - Customer and shipping info
         */
        checkout: function (checkoutData) {
            return fetch('/api/orders/checkout', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(checkoutData)
            }).then(response => response.json());
        },

        /**
         * Update order status (Admin only)
         * @param {string} orderId - Order ID
         * @param {string} status - New status
         */
        updateStatus: function (orderId, status) {
            const formData = new FormData();
            formData.append('orderId', orderId);
            formData.append('status', status);

            return fetch('/api/orders/status', {
                method: 'POST',
                body: formData
            }).then(response => response.json());
        },

        /**
         * Get order statistics
         */
        getStats: function () {
            return fetch('/api/orders/stats')
                .then(response => response.json());
        }
    },

    // ==================== AUTH API ====================
    auth: {
        /**
         * Login user
         * @param {string} email - User email
         * @param {string} password - User password
         */
        login: function (email, password) {
            return fetch('/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Email: email, Password: password })
            }).then(response => response.json());
        },

        /**
         * Register new user
         * @param {object} userData - Registration data
         */
        register: function (userData) {
            return fetch('/api/auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            }).then(response => response.json());
        },

        /**
         * Logout current user
         */
        logout: function () {
            return fetch('/api/auth/logout', {
                method: 'POST'
            }).then(response => response.json());
        },

        /**
         * Get current logged in user
         */
        getCurrentUser: function () {
            return fetch('/api/auth/current')
                .then(response => response.json());
        },

        /**
         * Check if user is authenticated
         */
        checkAuth: function () {
            return fetch('/api/auth/check')
                .then(response => response.json());
        }
    },

    // ==================== USERS API (Admin) ====================
    users: {
        /**
         * Get all users (Admin only)
         */
        getAll: function () {
            return fetch('/api/users')
                .then(response => response.json());
        },

        /**
         * Get user by ID (Admin only)
         * @param {number} id - User ID
         */
        getById: function (id) {
            return fetch(`/api/users/get/${id}`)
                .then(response => response.json());
        },

        /**
         * Get all customers (Admin only)
         */
        getCustomers: function () {
            return fetch('/api/users/customers')
                .then(response => response.json());
        },

        /**
         * Update user (Admin only)
         * @param {object} user - User data with ID
         */
        update: function (user) {
            return fetch('/api/users/update', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(user)
            }).then(response => response.json());
        },

        /**
         * Delete user (Admin only)
         * @param {number} id - User ID
         */
        delete: function (id) {
            return fetch(`/api/users/delete/${id}`, {
                method: 'POST'
            }).then(response => response.json());
        },

        /**
         * Toggle user active status (Admin only)
         * @param {number} id - User ID
         */
        toggleStatus: function (id) {
            return fetch(`/api/users/toggle-status/${id}`, {
                method: 'POST'
            }).then(response => response.json());
        },

        /**
         * Get user statistics (Admin only)
         */
        getStats: function () {
            return fetch('/api/users/stats')
                .then(response => response.json());
        }
    }
};

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ECommerceAPI;
}
