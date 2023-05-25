import axios from 'axios';

const http = axios.create({
  baseURL: "https://localhost:44315"
});

export default {
  postOrder(order) {
    return http.post('api/cart/checkout', order); 
  },
  GetListOfSpecialtyPizza(){
    return http.get('api/specialty');
  },
  GetListOfSides(){
    return http.get('api/sides');
  },
  GetListOfBeverages(){
    return http.get('api/beverage');
  },
  postReviews(reviewData){
    return http.post('api/review', reviewData);
  },
  getReviews(){
    return http.get('api/review');
  },
  addToCart(cartData){
    return http.post('api/cart/add-to-cart', cartData);
  },
  getCartByAnonymousId(anonymousId){
    return http.get(`api/cart/get-cart-by-anonymous-id/${anonymousId}`);
  },
  removeFromCart(id){
    return http.delete(`api/cart/${id}`);
  },
  clearCartWhenCheckOut(anonymousUserId){
    return http.delete(`api/cart/clear-cart/${anonymousUserId}`);
  }
};
