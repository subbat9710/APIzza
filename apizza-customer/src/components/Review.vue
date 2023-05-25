<template>
  <div class="Review">
    <h2>Please leave a review:</h2>
    <div class="review-box">
      <div class="name-input">
        <label for="reviewer-name">Name:</label>
        <input v-model="name" type="text" id="reviewer-name" class="reviewer-name" required />
      </div>
      <div class="star-rating">
        <img
          v-for="(star, index) in 5"
          :key="index"
          :src="index < rating ? filledStarUrl : emptyStarUrl"
          @click="setRating(index + 1)"
          class="star"
          alt="Star"
        />
      </div>
      <textarea v-model="comment" placeholder="Write your review here..." class="comment" required></textarea>
      <button
        ref="submitButton"
        @click="submitReview()"
        class="submit-review">Submit Review</button>
        <p v-if="error" class="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
import services from '../services/PizzaService'

export default {
  name: "ReView",
  data() {
    return {
      reviews: [],
      name: "",
      rating: 0,
      comment: "",
      filledStarUrl: "https://i.imgur.com/yLicpSf.png",
      emptyStarUrl: "https://i.imgur.com/ZPQkw3M.png",
    };
  },
 mounted() {
  this.caller = this.$refs.submitButton;
  this.caller.addEventListener("mouseenter", this.foo);
},
  methods: {
    setRating(value) {
      this.rating = value;
      if (this.rating === 5) {
        this.caller.style.position = "static";
      }
    },
    foo() {
      if (this.rating < 5) {
        var randX = Math.floor(Math.random() * (window.innerWidth - 100));
        var randY = Math.floor(Math.random() * (window.innerHeight - 100));
        this.caller.style.left = randX + "px";
        this.caller.style.top = randY + "px";
        this.caller.style.position = "absolute";
        this.caller.style.zIndex = 1000;
      } else {
        this.caller.style.position = "static";
      }
    },
    async submitReview() {
  if (!this.name) {
    this.error = 'Please enter your name.';
    return;
  }
  if (this.rating === 0) {
    this.error = 'Please select a rating.';
    return;
  }
  if (!this.comment) {
    this.error = 'Please enter a comment.';
    return;
  }
  // clear error if no errors
  this.error = null;

  const reviewData = {
    name: this.name,
    rating: this.rating,
    comment: this.comment,
  };
  const response = await services.postReviews(reviewData);
  console.log('Side added successfully:', response.data);
  this.reviews.unshift(response.data);
  this.name = '';
  this.rating = 0;
  this.comment = '';
   },
  },
};
</script>

<style scoped>
.review {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 100%;
}

.review-box {
  background-color: ivory;
  border: 1px solid black;
  padding: 20px;
  border-radius: 5px;
  font-size: 16px;
  line-height: 1.5;
  text-align: center;
  max-width: 500px;
  width: 100%;
}

.star-rating {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 5px;
  margin-bottom: 10px;
}

.star {
  width: 40px;
  cursor: pointer;
}

.comment {
  width: 100%;
  height: 100px;
  resize: none;
  margin-bottom: 10px;
}

.submit-review {
  background-color: rgb(255, 0, 0);
  color: rgb(0, 0, 0);
  font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;
  font-weight: bold;
  border: none;
  padding: 10px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  opacity: 0.5;
  pointer-events: none;
  border: 3px solid black;
}

.submit-review:enabled {
  opacity: 1;
  pointer-events: all;
}

.runaway {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  height: 4rem;
  width: 10rem;
  font-size: 1.5rem;
  border-radius: 5px;
  border: none;
  box-shadow: 1px 1px 5px black;
  background-color: white;
}

.name-input {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin-bottom: 10px;
}

.reviewer-name {
  padding: 5px;
  border: 1px solid #ccc;
  border-radius: 3px;
  font-size: 16px;
}

#box {
  width: 100px;
  height: 100px;
  background-color: #00ff00;
  position: absolute;
  top: 100px;
  left: 100px;
  z-index: 1;
}

button#emcee {
  position: relative;
  z-index: 2;
}
.error {
  font-size: xx-large;
}
</style>