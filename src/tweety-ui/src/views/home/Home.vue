<template>
  <div>
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <home-form v-if="isForm" :form="form" :submitData="submitData"></home-form>
    <home-result v-else :result="result"></home-result>
  </div>
</template>

<script src="./Home.js">
</script>
