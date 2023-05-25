<template>
  <div class="order-page">
    <h1>Customize your Pi</h1>
    <div class="order-content">
      <cart></cart>
      <weather></weather>
      <order-options></order-options>
    </div>
  </div>
</template>

<script>
import OrderOptions from "@/components/OrderOptions.vue"
import Cart from "@/components/Cart.vue";
import Weather from "@/components/Weather.vue";

export default {
  name: "OrderPage",
  components: {
    OrderOptions,
    Cart,
    Weather,
  },
};
</script>

<style scoped>
.order-page {
  width: 100%;
  padding: 20px;
  box-sizing: border-box;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.order-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  flex-wrap: wrap;
}
</style>