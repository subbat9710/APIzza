<template>
  <div class="home">
    <pizza-list></pizza-list>
    <cart></cart>
    <specialty-pizza></specialty-pizza>
    <custom-pizza></custom-pizza>
    <sides></sides>
    <beverages></beverages>
    <weather></weather>
    
  </div>
</template>

<script>
import Cart from "@/components/Cart.vue"
import PizzaList from "@/components/PizzaList.vue"
import SpecialtyPizza from "@/components/SpecialtyPizza.vue"
import CustomPizza from "@/components/CustomPizza.vue"
import Sides from "@/components/Sides.vue"
import Beverages from "@/components/Beverages.vue"
import Weather from "@/components/Weather.vue"



export default {
  name: 'HomeView',
  components: {
    PizzaList,
    SpecialtyPizza,
    CustomPizza,
    Sides,
    Beverages,
    Weather,
    Cart,
  }
}
</script>
