"use client";
import { getCategories } from "@/services/api/categoryAPI";
import { getProducts } from "@/services/api/productAPI";
import { CategoryDto } from "@/services/dto/categoryDto";
import { ProductDto } from "@/services/dto/productDto";
import { useEffect, useState } from "react";

export default function Home() {
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]); // Explicitly type as string array

  useEffect(() => {
    fetchCategories();
    fetchProducts();
  }, []);

  const fetchCategories = async () => {
    try {
      const response = await getCategories();
      const data = await response.json();
      setCategories(data.items);
    } catch (error) {
      console.error("Error fetching categories:", error);
    }
  };

  const fetchProducts = async () => {
    try {
      const response = await getProducts();
      const data = await response.json();
      setProducts(data.items);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };

  const handleCategoryChange = (
    event: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const selectedCategory = event.target.value;
    setSelectedCategories([selectedCategory]);
  };

  const filteredProducts =
    selectedCategories[0] === "" || selectedCategories.length === 0
      ? products
      : products.filter(
          (product) => product.categoryId.toString() === selectedCategories[0]
        );

  return (
    <div className="container">
      <h1 className="title">Product List</h1>
      <div className="categoryFilter">
        <label htmlFor="categories">Filter by Category:</label>
        <select
          id="categories"
          onChange={handleCategoryChange}
          value={selectedCategories[0] || ""}
          className="categorySelect"
        >
          <option value="">All Categories</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
      </div>
      <div className="productList">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Price</th>
              <th>Category</th>
            </tr>
          </thead>
          <tbody>
            {filteredProducts.map((product) => (
              <tr key={product.id}>
                <td>{product.name}</td>
                <td>${product.price}</td>
                <td>
                  {categories.find((cat) => cat.id === product.categoryId)
                    ?.name || "Unknown"}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
