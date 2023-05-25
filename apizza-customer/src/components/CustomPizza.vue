<template>
  <div class="outer-container">
    <h1><u>Custom Pi</u></h1>
    <div class="custom-pizza-container">
      <div v-for="(pizza, index) in pizzas" :key="index" class="pizza-box" @click="navigateToOrderPage">
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
          <img :src="pizza.imageUrl" :alt="pizza.name" class="pizza-image" />
        </div>
        <h3><u>{{ pizza.name }}</u></h3>
        <p>{{ pizza.description }}</p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "CustomPizza",
  data() {
    return {
      pizzas: [
        {
          name: "CUSTOM",
          imageUrl: "https://i.imgur.com/73g0r28.png",
          description: "Customize your pi. Feel free to get weird with it",
        },
      ],
    };
  },
  methods: {
    navigateToOrderPage() {
      this.$router.push({ name: "OrderPage" });
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
  margin-right: 9.5cm;
}

.custom-pizza-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  grid-gap: 20px;
  justify-items: center;
  margin-top: 20px;
  max-width: calc(100% - 350px);
}

.pizza-box {
  position: relative;
  width: 300px;
  height: 500px;
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

@media (max-width: 767px) {
  .custom-pizza-container {
    grid-template-columns: repeat(2, 1fr);
    padding-right: 0;
  }
}

@media (max-width: 480px) {
  .custom-pizza-container {
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

.image-container {
position: relative;
width: 100%;
height: 60%;
display: flex;
align-items: center;
justify-content: center;
margin-top: 20%;
}
</style>