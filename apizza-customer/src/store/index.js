import Vue from 'vue';
import Vuex from 'vuex'; 

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    cart: [],
    customPizzaCount: 0,
    customerOrder: {
      orderType: null,
      deliveryTime: null,
    },
  },
  mutations: {
    addToCart(state, payload) {
      const itemIndex = state.cart.findIndex(
        (item) => item.name === payload.name
      );
      if (itemIndex !== -1) {
        state.cart[itemIndex].quantity++;
      //  state.cart[itemIndex].id = payload.id; // Update the ID of the existing item
      } else {
        if (payload.type === 'custom') {
          payload.name = "Custom Pi";
        }
        state.cart.push({ ...payload, quantity: 1 });
      }
    },    
    setCart(state, cartItems) {
      state.cart = cartItems;
    },
    setSpecialtyPizzaData(state, data) {
      state.specialtyPizzaData = data;
    },
    removeItemFromCart(state, index) {
      state.cart.splice(index, 1);
    },
    setOrderType(state, orderType) {
      state.customerOrder.orderType = orderType;
    },
    addReview(state, review) {
      state.reviews.unshift(review);
    },
    CLEAR_CART(state){
      state.cart = [];
    },
    UPDATE_ORDER_STATUS(state, status) {
      state.orderStatus = status;
    },
  },
  actions: {
    clearCart(context){
      context.commit("CLEAR_CART");
    },
  }
});