import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    result: {
      count: 0,
      tweet: []
    }
  },
  mutations: {
    setResult(state, { count, tweet }) {
      state.result.count = count;
      state.result.tweet = tweet;
    }
  },
  actions: {
    saveResult({ commit }, { count, tweet }) {
      commit("setResult", { count, tweet });
    }
  }
});
