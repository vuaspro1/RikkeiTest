import axiosClient from "@/utils/request";

export const getProducts = async () => {
  const response = await axiosClient.get("/products");
  return response.data;
};
