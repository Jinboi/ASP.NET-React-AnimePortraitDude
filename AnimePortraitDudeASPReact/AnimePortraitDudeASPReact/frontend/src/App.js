import React, { useState, useEffect } from "react";
import axios from "axios";

function App() {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        axios.get("http://localhost:5212/api/products") // Updated to correct port
            .then(response => {
                console.log("Fetched products:", response.data);
                setProducts(response.data);
            })
            .catch(error => {
                console.error("Error fetching products:", error);
                setError("Failed to fetch products. Check the console for details.");
            });
    }, []);

    return (
        <div>
            <h1>Products</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            <ul>
                {products.map(product => (
                    <li key={product.id}>{product.name} - ${product.price.toFixed(2)}</li>
                ))}
            </ul>
        </div>
    );
}

export default App;