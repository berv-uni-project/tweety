import axios from "axios";
import HomeForm from "@/components/home-form/HomeForm.vue";
import HomeResult from "@/components/home-result/HomeResult.vue";
import services from "@/services/index.js";

export default {
  name: "home",
  data() {
    return {
      form: {
        search: "",
        dinkes: [],
        dinpem: [],
        dinsos: [],
        dinpen: [],
        dinbina: [],
        method: ""
      },
      result: {
        count: 0,
        query: []
      },
      isLoading: false,
      activeTab: 0
    };
  },
  components: {
    "home-form": HomeForm,
    "home-result": HomeResult
  },
  methods: {
    submitData() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.isLoading = true;
          axios
            .post(
              services.search,
              {
                Name: this.form.search,
                DinasKesehatan: this.form.dinkes.join(","),
                DinasPemuda: this.form.dinpem.join(","),
                DinasSosial: this.form.dinsos.join(","),
                DinasPendidikan: this.form.dinpen.join(","),
                DinasBinamarga: this.form.dinbina.join(","),
                IsKMP: this.form.method
              },
              {
                responseType: "json"
              }
            )
            .then(response => {
              this.isLoading = false;
              if (response.status === 200) {
                this.$toast.open({
                  message: "Success",
                  type: "is-success",
                  position: "is-bottom"
                });
                this.result.count = response.data.result.count;
                this.result.query = response.data.result.data.query;
                this.activeTab = 1;
              } else {
                this.$toast.open({
                  message: "Form is not valid! Please check the fields.",
                  type: "is-danger",
                  position: "is-bottom"
                });
              }
            })
            .catch(error => {
              this.isLoading = false;
              this.$toast.open({
                message: error.message,
                type: "is-danger",
                position: "is-bottom"
              });
            });
        } else {
          this.$toast.open({
            message: "Form is not valid! Please check the fields.",
            type: "is-danger",
            position: "is-bottom"
          });
        }
      });
    }
  }
};
