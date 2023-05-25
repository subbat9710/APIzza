import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store/index';
import * as THREE from "three";
import VueParticles from 'vue-particles';

Vue.config.productionTip = false;
Vue.use(VueParticles)

window.THREE = THREE;

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app');