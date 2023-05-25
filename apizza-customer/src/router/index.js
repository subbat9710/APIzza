import Vue from 'vue';
import Router from 'vue-router';
import HomeView from '../views/HomeView.vue';
import Cart from '../components/Cart.vue';
import Weather from '../components/Weather.vue';
import OrderPage from "@/views/OrderPage.vue";
import CheckOut from "@/views/CheckOut.vue";
import ReviewPage from "@/views/ReviewPage.vue";

Vue.use(Router);

const routes = [
  { path: '/', component: HomeView, name: 'HomeView' },
  { path: '/cart', component: Cart, name: 'Cart' },
  { path: '/weather', component: Weather, name: 'Weather' },
  { path: '/order', component: OrderPage, name: "OrderPage"}, 
  { path: '/checkout', component: CheckOut, name: "CheckOut"}, 
  {path: "/review", component: ReviewPage, name: "ReviewPage",},
];

const router = new Router({
  mode: 'history',
  routes,
});

export default router;
