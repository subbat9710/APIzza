<template>
  <div class="review-page">
    <button @click="redirectToHome" class="new-add-to-cart-button shrink-border go-back-button">Home</button>
    <h1>Thank you for your order!</h1>
    <Review />
    <div class="previous-reviews">
      <PreviousReviews />
    </div>
  </div>
</template>

<script>
import Review from "@/components/Review.vue";
import PreviousReviews from "@/components/PreviousReviews.vue";

export default {
  name: "ReviewPage",
  components: {
    Review,
    PreviousReviews,
  },
  methods: {
    redirectToHome(){
      this.$router.push('/');
    }
  }
};
</script>

<style scoped>
.review-page {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 100%;
  text-align: center;
}

.go-back-button {
  font-family: 'Montserrat', sans-serif;
  text-transform: uppercase;
  position: fixed;
  top: 20px;
  left: 20px;
  border: none;
  font-size: 18px;
  transition: color 0.5s, transform 0.2s, background-color 0.2s;
  outline: none;
  border-radius: 3px;
  margin: 0 10px;
  padding: 23px 33px;
  border: 3px solid black;
  background-color: red;
  color: black;
  font-weight: bold;
  z-index: 1000;
}

.go-back-button:hover {
  background-color: red;
  box-shadow: none;
  color: black;
}

.go-back-button:hover::after {
  opacity: 1;
  transform: scaleX(1) scaleY(1);
}

.go-back-button::after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: transparent;
  border: 3px solid red;
  opacity: 0;
  z-index: -1;
  transform: scaleX(1.1) scaleY(1.3);
  transition: transform 0.3s, opacity 0.3s;
}
.previous-reviews {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 0 auto;
  padding-top: 20px; /* Add padding to the top to create space for the heading */
}

/* Media query for responsive layout */
@media screen and (max-width: 768px) {
  .review-page h1 {
    margin-left: 0;
  }

  .go-back-button {
    top: 10px;
    left: 10px;
    padding: 15px 25px;
    font-size: 16px;
  }

  .previous-reviews {
    margin-top: 20px;
    width: 100%;
    padding: 0 10px;
  }
}
</style>