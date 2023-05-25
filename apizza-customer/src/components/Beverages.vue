<template>
  <div class="outer-container">
    <h1><u>Beverages</u></h1>
    <div class="beverages-container">
      <div v-for="(beverage, index) in beverages" :key="index" class="beverage-box" @click="addBeverageToCart(beverage)">
        <video class="background-video" src="https://static.videezy.com/system/resources/previews/000/040/424/original/star_burst_2.mp4" autoplay loop muted></video>
        <img :src="beverage.imageUrl" :alt="beverage.itemName" class="beverage-image spin" />
        <h3><u>{{ beverage.itemName }}</u></h3>
        <p>{{ beverage.description }}</p>
        <p>${{ beverage.price }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import beveragesList from '../services/PizzaService'
import { v4 as uuidv4 } from 'uuid';

const ANONYMOUS_SESSION_ID_KEY = 'anonymousSessionId';
export default {
  name: "BeveRages",
  data() {
    return {
      beverages: [],
    };
  },
  created(){
    beveragesList.GetListOfBeverages().then((response) => {
      this.beverages = response.data;
    })
  },
methods: {
    async addBeverageToCart(beverage) {
      console.log("Beverage clicked:", beverage.itemName);
      const item = {
        type: "Beverage",
        name: beverage.itemName,
        //size: "",
        quantity: 1,
        price: beverage.price, // Set the price property dynamically
        options: {},
      };
      //this.$store.commit("addToCart", item);

       // Send the updated cart to the server
        let anonymousSessionId = localStorage.getItem(ANONYMOUS_SESSION_ID_KEY);
          if (!anonymousSessionId) {
            anonymousSessionId = uuidv4();
            localStorage.setItem(ANONYMOUS_SESSION_ID_KEY, anonymousSessionId);
          }

          const cartData = {
          items: [item],
          AnonymousId: anonymousSessionId,
        };

        try {
          const response = await beveragesList.addToCart(cartData);
          console.log('Added to cart:', response.data);
          const id = response.data.cartItemId;
          this.$store.commit("addToCart", { ...item, id });
          this.navigateToCheckOut();
        } catch (error) {
          console.log('Error while adding to cart:', error);
        }
      },
   },
};
</script>


<style scoped>
.outer-container {
  width: 100%;
  padding: 20px;
  box-sizing: border-box;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
  margin-right: 8cm;
}

.beverages-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  grid-gap: 20px;
  justify-items: center;
  margin-top: 20px;
  max-width: calc(100% - 350px);
}

.beverage-box {
  position: relative;
  width: 300px;
  background-color: transparent;
  padding: 20px;
  border-radius: 5px;
  color: rgb(255, 255, 255);
  text-align: center;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  align-items: center;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.background-video {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  z-index: -1;
}

.beverage-image {
  width: 100%;
  height: auto;
  border-radius: 5px;
  margin-bottom: 10px;
}

@media (max-width: 767px) {
  .beverages-container {
    grid-template-columns: repeat(2, 1fr);
    padding-right: 0;
  }
}

@media (max-width: 480px) {
  .beverages-container {
    grid-template-columns: 1fr;
    padding-right: 0;
  }
}

.spin {
  transition: transform 0.5s ease-in-out;
}

.spin:hover {
  transform: rotateY(360deg);
}
</style>