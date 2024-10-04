import axiosClient from "@/utils/request";

export const getCategories = async () => {
  const response = await axiosClient.get("/categories");
  return response.data;
};
