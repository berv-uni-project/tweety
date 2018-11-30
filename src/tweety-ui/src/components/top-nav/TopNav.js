export default {
  name: "top-nav",
  methods: {
    burgerClick() {
      const burger = document.getElementById("burger");
      if (burger) {
        burger.classList.toggle("is-active");
        const target = burger.dataset.target;
        if (target) {
          const $target = document.getElementById(target);
          if ($target) {
            $target.classList.toggle("is-active");
          }
        }
      }
    }
  },
  watch: {
    $route() {
      const burger = document.getElementById("burger");
      if (burger) {
        burger.classList.remove("is-active");
        const target = burger.dataset.target;
        if (target) {
          const $target = document.getElementById(target);
          if ($target) {
            $target.classList.remove("is-active");
          }
        }
      }
    }
  }
};
