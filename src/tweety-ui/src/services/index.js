const baseURL = process.env.VUE_APP_BASE_URL;

const services = {
  search: baseURL + "api/v1/tweety/find"
};

export default services;
