import axios from "axios";
import qs from "qs";
import * as env from "../../env";

export interface ResponseSuccess<T> {
  success?: boolean;
  data: T;
  message?: string;
}

export interface ResponseError {
  status: number;
  title: string;
  errors: {
    [key: string]: string[];
  };
}

const axiosClient = axios.create({
  baseURL: env.API_URL,
  headers: {
    "content-type": "multipart/form-data",
    // 'content-type': 'application/json',
    // "Content-Type": "application/x-www-form-urlencoded"
    // 'Authorization': 'Bearer ' + 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJjb3JlIiwiYXVkIjoibGlmby52biIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiM2Q0NWM4MWItNzUwZS00MDg0LTlkMmEtMzYyM2RlMDRjYzI1IiwiaWF0IjoxNzI2MjEwNTcwLCJleHAiOjE3MzEzOTQ1NzB9.sDG4gGcrX6EccPay-hrbW8X5luzZ7MDSmhcqYzlPVTJ8EMkf_Tv9LTVQKDyD5xE5H8dzl2kLvPin3tUAP50hbA'
  },
  paramsSerializer: {
    serialize: (params) => {
      return qs.stringify(params, { arrayFormat: "repeat", allowDots: true });
    },
  },
});

axiosClient.interceptors.request.use(
  async (config) => {
    //handle token here
    // const token = tokenService.getToken();
    // config.headers!.Authorization = 'Bearer ' + token;
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosClient.interceptors.response.use(
  (response) => {
    if (response && response.data) {
      return response.data;
    }

    return response;
  },
  (error) => {
    // console.error(error);
    if (
      error.response &&
      error.response.data &&
      error.response.data.error &&
      (error.response.data.session === false ||
        error.response.data.session === "false")
    ) {
    } else if (
      error.response &&
      error.response.data &&
      error.response.data.error &&
      error.response.data.error.message
    ) {
      // toastMessage(error.response.data.error.message, 1);
      return error.response.data;
    } else if (error.response && error.response.status === 401) {
      return error.response.data;
      // alert("Bạn đã hết phiên đăng nhập, vui lòng đăng nhập lại!");
      // tokenService.removeToken();
      // window.location = "/";
      // window.location.href = "/";
    } else return error.response.data;
    // return Promise.reject(error);
  }
);

export default axiosClient;
