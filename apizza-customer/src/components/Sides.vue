<template>
  <div class="outer-container">
    <h1><u>Sides</u></h1>
    <div class="sides-container">
      <div v-for="(side, index) in sides" :key="index" class="side-box" @click="addSideToCart(side)">
        <video class="background-video" src="https://static.videezy.com/system/resources/previews/000/040/424/original/star_burst_2.mp4" autoplay loop muted></video>
        <div class="image-container">
          <div class="vortex-wrapper">
            <div class="arc">
              <div class="arc">
                <div class="arc">
                  <div class="arc">
                    <div class="arc"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <img :src="side.imageUrl" :alt="side.name" class="side-image" />
        </div>
        <h3><u>{{ side.itemName }}</u></h3>
        <p>{{ side.description }}</p>
        <p>${{ side.price }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import PizzaServices from '../services/PizzaService'
import { v4 as uuidv4 } from 'uuid';

const ANONYMOUS_SESSION_ID_KEY = 'anonymousSessionId';

export default {
  name: "SiDes",
  data() {
    return {
      sides: [],
    };
  },
  created(){
    PizzaServices.GetListOfSides().then((response) => {
       this.sides = response.data;
     });
  },
  methods: {
    async addSideToCart(side) {
      console.log("Side clicked:", side.itemName);
      const item = {
        type: "Side",
        name: side.itemName,
        //size: "",
        quantity: 1,
        price: side.price, // Set the price property dynamically
        options: {}, // You can add options here if needed
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
          const response = await PizzaServices.addToCart(cartData);
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
  width: calc(100% - 8cm);
  padding: 10px;
  box-sizing: border-box;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.sides-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  grid-gap: 10px;
  justify-items: center;
  margin-top: 20px;
}

.side-box {
  position: relative;
  width: 100%;
  max-width: 300px;
  height: 400px;
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

.side-image {
  position: relative;
  z-index: 1;
  width: 300px;
  height: 250px;
  border-radius: 5px;
  margin-bottom: 10px;
  object-fit: cover;
  transition: all 0.1s ease-in-out;
}

.side-image:hover {
  animation: shake 0.5s linear infinite;
}

@keyframes shake {
  0% { transform: translate(1px, 1px) rotate(0deg); }
  10% { transform: translate(-1px, -2px) rotate(-1deg); }
  20% { transform: translate(-3px, 0px) rotate(1deg); }
  30% { transform: translate(1px, 2px) rotate(0deg); }
  40% { transform: translate(1px, -1px) rotate(1deg); }
  50% { transform: translate(-1px, 2px) rotate(-1deg); }
  60% { transform: translate(-1px, 1px) rotate(0deg); }
  70% { transform: translate(1px, 1px) rotate(-1deg); }
  80% { transform: translate(-1px, -1px) rotate(1deg); }
  90% { transform: translate(1px, 2px) rotate(0deg); }
  100% { transform: translate(1px, -2px) rotate(-1deg); }
}

@media (max-width: 1200px) {
  .sides-container {
    justify-content: space-around;
  }
}

@media (max-width: 767px) {
  .sides-container {
    justify-content: space-around;
  }
}

@media (max-width: 480px) {
  .sides-container {
    flex-direction: column;
    align-items: center;
  }

  .side-box {
    margin-bottom: 20px;
  }
}
</style>