<template>
    <div class="box">
    <h1 class="title has-text-centered">Tweety</h1>
    <form @submit.prevent="submitData">
      <b-field
        label="Search"
        :type="{'is-danger': errors.has('search')}"
        :message="errors.first('search')"
      >
        <b-input
          v-model="form.search"
          name="search"
          v-validate="'required'"
          placeholder="Search Keyword in Twitter"
        />
      </b-field>
      <b-field label="Dinas Kesehatan">
        <b-taginput
          v-model="form.dinkes"
          maxtags="5"
          type="is-info"
          ellipsis
          icon="tag"
          placeholder="Search Tags for Dinas Kesehatan"
        ></b-taginput>
      </b-field>
      <b-field label="Dinas Sosial">
        <b-taginput
          v-model="form.dinsos"
          maxtags="5"
          type="is-info"
          ellipsis
          icon="tag"
          placeholder="Search Tags for Dinas Sosial"
        ></b-taginput>
      </b-field>
      <b-field label="Dinas Pendidikan">
        <b-taginput
          v-model="form.dinpen"
          maxtags="5"
          type="is-info"
          ellipsis
          icon="tag"
          placeholder="Search Tags for Dinas Pendidikan"
        ></b-taginput>
      </b-field>
      <b-field label="Dinas Pemuda dan Olahraga">
        <b-taginput
          v-model="form.dinpem"
          maxtags="5"
          type="is-info"
          ellipsis
          icon="tag"
          placeholder="Search Tags for Pemuda dan Olahraga"
        ></b-taginput>
      </b-field>
      <b-field label="Dinas Binamarga dan Pengairan">
        <b-taginput
          v-model="form.dinbina"
          maxtags="5"
          type="is-info"
          ellipsis
          icon="tag"
          placeholder="Search Tags for Dinas Binamarga dan Pengairan"
        ></b-taginput>
      </b-field>

      <b-field
        label="Select Method"
        :addons="false"
        :type="{'is-danger': errors.has('isKMP')}"
        :message="errors.first('isKMP')"
      >
        <b-radio v-model="form.method" name="isKMP" native-value="true" v-validate="'required'">KMP</b-radio>
        <b-radio v-model="form.method" name="isKMP" native-value="false" v-validate="'required'">Booyer-Moore</b-radio>
      </b-field>
      <button class="button is-primary" type="submit">Submit</button>
    </form>
  </div>
</template>

<script src="./HomeForm.js">
</script>
