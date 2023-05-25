
<template>
  <div class="outer-container">
    <h1><u>Specialty Pi</u></h1>
    <div class="specialty-pizza-container">
      <div
        v-for="(pizza, index) in pizzas"
        :key="index"
        class="pizza-box"
        @click="addPizzaToCart(pizza)"
      >
        <video class="background-video" src="https://static.videezy.com/system/resources/previews/000/040/424/original/star_burst_2.mp4" autoplay loop muted style="z-index:-1"></video>
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
          <img :src="pizza.imageUrl" :alt="pizza.name" class="pizza-image" />
        </div>
        <h3><u>{{ pizza.name }}</u></h3>
        <p>{{ pizza.description }}</p>
        <p>${{ pizza.price }}</p>
        
      </div>
    </div>
  </div>
</template>

<script>
import specialtyPizzaList from '../services/PizzaService'
import { v4 as uuidv4 } from 'uuid';

const ANONYMOUS_SESSION_ID_KEY = 'anonymousSessionId';
export default {
  name: "SpecialtyPizza",
  data() {
    return {
      pizzas: [],
    };
  },
  // created(){
  //    specialtyPizzaList.GetListOfSpecialtyPizza().then((response) => {
  //      this.pizzas = response.data;
  //    });
  // },
  mounted() {
    specialtyPizzaList.GetListOfSpecialtyPizza().then((response) => {
      this.pizzas = response.data;
    });
  },
 methods: {
  async addPizzaToCart(pizza) {
    console.log("Pizza clicked:", pizza.name);
    
    const item = {
      type: "Specialty Pizza", // Set the type property to "Specialty Pizza"
      name: pizza.name,
      //size: "",
      quantity: 1,
      price: pizza.price, // Set the price property dynamically
      options: {},
    };

    // Wait for the anonymous session ID to be retrieved from localStorage
    const anonymousSessionId = await new Promise((resolve) => {
      let sessionId = localStorage.getItem(ANONYMOUS_SESSION_ID_KEY);
      if (!sessionId) {
        sessionId = uuidv4();
        localStorage.setItem(ANONYMOUS_SESSION_ID_KEY, sessionId);
      }
      resolve(sessionId);
    });

    // Send the updated cart to the server
    const cartData = {
      items: [item],
      AnonymousId: anonymousSessionId,
    };
    try {
      const response = await specialtyPizzaList.addToCart(cartData);
      console.log('Added to cart:', response.data);
      const id = response.data.cartItemId;  //get the cartItem id from the server
      console.log('CartItem updated id:', response.data.cartItemId)
      this.$store.commit("addToCart", { ...item, id });
      this.navigateToCheckOut();
    } catch (error) {
      console.log("Error while adding to cart:", error);
    }
  },
  },
};
</script>

<style scoped>
.outer-container {
  width: 100%;
  box-sizing: border-box;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
  margin-right: 9.5cm;
}

.specialty-pizza-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  grid-gap: 20px; /* Updated grid-gap */
  justify-items: center;
  align-content: center;
  margin-top: 40px;
  max-width: calc(100% - 350px);
}

.pizza-box {
  position: relative;
  width: 325px; /* Updated width */
  height: 500px;
  border-radius: 5px;
  color: rgb(255, 255, 255);
  text-align: center;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  align-items: center;
}

.pizza-box-wrapper {
  position: relative;
  width: 300px;
  min-height: 500px;
  padding: 20px;
  box-sizing: border-box;
}


@keyframes rotate {
  0% {
    transform: rotate(90deg);
  }
  100% {
    transform: rotate(810deg);
  }
}

.pizza-image {
   position: relative;
  z-index: 1;
  width: 80%;
  height: auto;
  border-radius: 5px;
  margin-bottom: 20px;
}

.pizza-image:hover {
  transform: rotate(360deg);
  transition: transform 1s ease;
}

h3,
p {
  word-wrap: break-word;
}

@media (max-width: 1024px) {
  .specialty-pizza-container {
    grid-template-columns: repeat(3, 1fr);
    padding-right: 0;
  }
}

@media (max-width: 767px) {
  .specialty-pizza-container {
    grid-template-columns: repeat(2, 1fr);
    padding-right: 0;
  }
}

@media (max-width: 480px) {
  .specialty-pizza-container {
    grid-template-columns: 1fr;
    padding-right: 0;
  }
}
.vortex-wrapper {
  position: absolute;
  width: 120%;
  height: 120%;
top: -13%;
left: -10%;
z-index: 1;
}

.arc:before, .arc:after {
display: block;
position: absolute;
width: 100%;
height: 100%;
border: 4px solid #000;
top: -4px;
left: -4px;
border-color: rgb(255, 255, 255) transparent transparent transparent;
border-radius: 50%;
z-index: -1;
content: "";
}

.arc:after {
border-color: transparent rgba(255, 227, 69, 0.932) hsl(194, 93%, 49%) rgb(255, 0, 0);
}

.arc {
position: absolute;
display: block;
width: 85%;
height: 85%;
margin: 7.5%;
border-radius: 50%;
animation: 15s rotate linear infinite;
box-shadow: 0px 0px 0px 2px #fff2;
}

@keyframes rotate {
  0% {
    transform: rotate(90deg);
  }
  100% {
    transform: rotate(810deg);
  }
}

.background-video {
  position: absolute;
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center center; /* Center the video within the container */
  z-index: 0; /* Place the video behind the vortex effect */
}

.image-container {
position: relative;
width: 300px;
height: 300px;
display: flex;
align-items: center;
justify-content: center;
margin-top: 20%;
}
</style>