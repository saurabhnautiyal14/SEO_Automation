import axios, { AxiosResponse } from "axios";

axios.defaults.baseURL = "http://localhost:5000/api";

const responseBody = (response: AxiosResponse) => response.data;

//TODO Add error handling.
const requests = {
  get: (url: string) => axios.get(url).then(responseBody)
};

const params = {
  value: "HELLO"
};

const searchEngine = {
  details: (id: string) => requests.get(`/searchEngine`)
};

export default {
  searchEngine
};
