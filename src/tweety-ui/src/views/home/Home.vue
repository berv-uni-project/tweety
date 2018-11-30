<template>
  <div>
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <b-tabs v-model="activeTab" position="is-centered">
      <b-tab-item label="Search">
        <home-form :form="form" :submitData="submitData"></home-form>
      </b-tab-item>
      <b-tab-item label='Result'>
        <home-result :result="result"></home-result>  
      </b-tab-item>
    </b-tabs>
  </div>
</template>

<script src="./Home.js">
</script>
