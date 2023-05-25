<template>
  <div>
    <div class="cart">
       <video class="video-background" autoplay loop muted>
        <source src="https://static.videezy.com/system/resources/previews/000/040/424/original/star_burst_2.mp4" type="video/mp4" />
      </video>
      <h2><u>Your Cart:</u></h2>
      <div class="cart-items-container">
        <ul class="cart-items">
          <li v-for="(item, index) in cartItems" :key="index" class="cart-item">
            <div class="cart-item-content">
              {{item.quantity}} - {{ item.name }}
              <!-- <ul>
                <li>
                  &#8226; {{ item.size.size }}
                </li>
              </ul> -->
              <ul>
                <li v-for="topping in item.toppings" :key="topping">
                  &#8226; {{ topping.topping }}
                </li>
              </ul>
              <ul>
                <li v-for="(optionValue, optionKey) in item.options" :key="optionKey">
                  <span>{{ optionKey }}: </span>
                  <span v-if="Array.isArray(optionValue)">{{ optionValue.join(', ') }}</span>
                  <span v-else>{{ optionValue }}</span>
                </li>
              </ul>
            </div>
            
            <span class="delete-item" @click="removeItemFromCart(index)">x</span>
          </li>
        </ul>
        
        <button @click="checkOut()" class="checkout-button shrink-border">Checkout</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex';
import PizzaService from '@/services/PizzaService';
import { v4 as uuidv4 } from 'uuid';

const ANONYMOUS_SESSION_ID_KEY = 'anonymousSessionId';

export default {
  name: 'Cart',
  computed: {
    cartItems: {
      get() {
        console.log('Added items are:', this.$store.state.cart);
        return this.$store.state.cart;
      },
      set(value) {
        this.$store.commit('setCart', value);
      }
    }
  },

  data() {
    return {
      isDelivery: false,
    };
  },
  mounted() {
    let anonymousSessionId = localStorage.getItem(ANONYMOUS_SESSION_ID_KEY);
    if (!anonymousSessionId) {
      anonymousSessionId = uuidv4();
      localStorage.setItem(ANONYMOUS_SESSION_ID_KEY, anonymousSessionId);
    }
    PizzaService.getCartByAnonymousId(anonymousSessionId)
      .then(response => {
        if (response.status === 200) {
          console.log('Added cart items from the server:', response.data);
          this.setCart(response.data);
        }
      })
      .catch(error => console.log(error));
    },
    watch: {
      isDelivery(newVal) {
        this.toggleDelivery(newVal);
    },
  },
  methods: {
    ...mapActions(['clearCart']), // Map the submitOrder action from Vuex to a local method
    toggleDelivery(isDelivery) {
      if (isDelivery) {
        console.log("Delivery selected");
      } else {
        console.log("Pickup selected");
      }
    },
    setCart(cartItems) {
    this.cartItems = cartItems;
    },
    navigateToCheckOut() {
      if (this.$route.name !== "CheckOut") {
        this.$router.push({ name: "CheckOut" });
      }
    },
    removeItemFromCart(index) {
      const item = this.cartItems[index];
      const anonymousSessionId = localStorage.getItem('anonymousSessionId');
      console.log('Itemid:', item.id);
      console.log('AnonymousId', anonymousSessionId);
      PizzaService.removeFromCart(item.id, anonymousSessionId)
        .then(() => {
          console.log('Itemid removed:', item.id);
          //remove the item from the cartItems array
          this.cartItems.splice(index, 1);
          // update the cart in the store
          this.$store.commit('removeFromCart', index);
        })
        .catch(error => console.log(error));
    },
    async checkOut(){
      this.navigateToCheckOut();
     },
  },
};
</script>

<style scoped>
.cart {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 250px;
  height: 600px;
  background-color: transparent;
  padding: 15px;
  border-radius: 10px;
  box-shadow: 0 8px 15px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  font-size: 0.3cm;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Added shadow effect */
 overflow: hidden;
 text-align: center;
}

.cart h2 {
  color: white;
}

.cart-item-content {
  color: white;
}
.cart-item-content ul li {
  margin: 6px;
}

.video-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  z-index: -1;
}

.cart-items {
  max-height: 400px; /* Adjust this value to fit your desired cart height */
  overflow-y: auto;
}

.cart-items-container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.cart-items li {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  margin-bottom: 10px;
}

.cart-items li ul {
  margin-top: 0;
  margin-bottom: 5px;
}

.cart-items li ul li {
  margin-bottom: 0;
}

.cart-item {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 10px;
  padding-bottom: 10px;
  border-bottom: 1px solid rgb(255, 255, 255);
}

.delete-item {
  color: rgb(190, 31, 31);
  cursor: pointer;
  font-weight: bold;
  margin-left: 5px;
  user-select: none;
}

.checkout-button {
  margin-top: 20px;
  padding: 10px 20px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}

.checkout-button:hover {
  background-color: #3e8e41;
}

.toggle-container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.toggle {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 34px;
  margin-bottom: 0.5rem;
}

.toggle input {
  display: none;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: 0.4s;
  transition: 0.4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 26px;
  width: 26px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  -webkit-transition: 0.4s;
  transition: 0.4s;
}

.slider.round {
  border-radius: 34px;
}

.slider.round:before {
  border-radius: 50%;
}

input:checked + .slider {
  background-color: #2196f3;
}

input:checked + .slider:before {
  -webkit-transform: translateX(26px);
  -ms-transform: translateX(26px);
  transform: translateX(26px);
}

button {
  font-family: 'Montserrat', sans-serif;
  text-transform: uppercase;
  position: relative;
  border: none;
  font-size: 18px;
  transition: color 0.5s, transform 0.2s, background-color 0.2s;
  outline: none;
  border-radius: 3px;
  margin: 0 10px;
  padding: 23px 33px;
  border: 3px solid white;
}

button:active {
  transform: translateY(3px);
}

button::after,
button::before {
  border-radius: 3px;
}

.shrink-border {
  background-color: transparent;
  color: rgb(255, 255, 255);  /* pre hover - text color*/
  font-weight: bold;
}

.shrink-border:hover {
background-color: transparent;
box-shadow: none;
color: rgb(255, 255, 255); /* Post hover - text color / */
font-weight: bold;
}

.shrink-border::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border: 3px solid rgb(255, 255, 255); /*Pre hover border color */
  transition: opacity 0.3s, border 0.3s;
}

.shrink-border:hover::before {
  opacity: 0;
}

.shrink-border::after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgb(9, 241, 47); /* Post hover background */
  border: 3px solid rgb(255, 255, 255); /* Post hover border*/
  opacity: 0;
  z-index: -1;
  transform: scaleX(1.1) scaleY(1.3);
  transition: transform 0.3s, opacity 0.3s;
}

.shrink-border:hover::after {
  opacity: 1;
  transform: scaleX(1) scaleY(1);
}
</style>


