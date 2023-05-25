<template>
  <div class="previous-reviews-container">
    <h2 class="previous-reviews-heading">Previous 5-star reviews:</h2>
    <div class="previous-reviews">
      <ul class="previous-reviews-list">
        <li v-for="(review, index) in displayedReviews" :key="index" class="previous-review-box">
          <div class="review-header">
            <h3 class="review-name">{{ review.name }}</h3>
            <img src="https://i.imgur.com/xfLZhsO.png" alt="Five stars" class="review-stars">
          </div>
          <div class="review-content-container">
            <p class="review-content">{{ review.comment }}</p>
            <p class="review-date">{{ formatDate(review.created) }}</p>
          </div>
        </li>
      </ul>
      <div class="show-more-button-container">
        <button v-if="displayedReviews.length < reviews.length" @click="showMore" class="show-more-button">Show More Reviews</button>
      </div>
    </div>
    <div class="average-review-box">
      <h2>Avg APIzza Review Score</h2>
      <img src="https://i.imgur.com/xfLZhsO.png" alt="Five stars" class="review-stars">
      <p>From {{ reviews.length }} reviews</p>
    </div>
  </div>
</template> 

<script>
import services from '../services/PizzaService'
export default {
  name: "PreviousReviews",
  data() {
    return {
      reviews: [],
      displayedReviews: [],
      reviewsPerPage: 2
    };
  },
  created() {
    services.getReviews().then((response) => {
      this.reviews = response.data;
      this.showMore();
    })
  },
  methods: {
    formatDate(date) {
      const options = { year: 'numeric', month: 'short', day: 'numeric' };
      return new Date(date).toLocaleDateString('en-US', options);
    },
    showMore() {
      const startIndex = this.displayedReviews.length;
      const endIndex = startIndex + this.reviewsPerPage;
      this.displayedReviews = [...this.displayedReviews, ...this.reviews.slice(startIndex, endIndex)];
    },
  },
};
</script>


<style>
  .previous-reviews-container {
    margin: 0 auto;
    max-width: 800px;
  }

  .previous-reviews-heading {
    margin-bottom: 20px;
  }

  .previous-review-box {
    margin-bottom: 20px;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #fff;
  }

  .review-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
  }

  .review-name {
    margin: 0;
    font-size: 18px;
    font-weight: bold;
  }

  .review-stars {
    width: 80px;
    height: 16px;
  }

  .review-content-container {
    margin-left: 20px;
  }

  .review-content {
    margin-bottom: 10px;
    font-size: 16px;
    line-height: 1.5;
  }

  .review-date {
    margin: 0;
    font-size: 14px;
    color: #666;
  }

  .average-review-box {
  position: fixed;
  bottom: 10px;
  right: 10px;
  background-color: white;
  padding: 10px;
  border-radius: 5px;
  box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3);
}
.show-more-button-container {
    width: 100%;
    text-align: center;
    margin: 20px;
  }

  .show-more-button {
    padding: 10px;
    background-color: #333;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }

  .show-more-button:hover {
    background-color: #555;
  }
</style>