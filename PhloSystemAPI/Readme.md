### Documentation

## 1. Project Overview

This WebAPI project provides an interface to query products with filtering capabilities such as minimum/maximum price, size, and highlights within the product descriptions. It follows a clean architecture with the segregation of business logic, data access, and API layer for scalability and maintainability.

---

## 2. API Endpoints

### **1. Filter Products**
- **Endpoint**: `/api/products/filter`
- **Method**: `GET`
- **Description**: Filters the product list based on query parameters like price, size, and highlighted keywords.
  
#### **Query Parameters**:
- `minprice` (optional, decimal): Minimum price to filter products.
- `maxprice` (optional, decimal): Maximum price to filter products.
- `size` (optional, string): Product size to filter products.
- `highlight` (optional, string): Comma-separated words to highlight in the product description.

#### **Sample Request**:
```http
GET /api/products/filter?minprice=10&maxprice=100&size=M&highlight=sale,discount
```

#### **Sample Response**:
```json
{
  "products": [
    {
      "title": "Sample Product 1",
      "description": "This product is on <em>sale</em>.",
      "price": 50,
      "sizes": ["S", "M", "L"]
    },
    {
      "title": "Sample Product 2",
      "description": "This is a <em>discount</em> offer.",
      "price": 80,
      "sizes": ["M", "L"]
    }
  ],
  "productFilter": {
    "minPrice": 50,
    "maxPrice": 80,
    "sizes": ["S", "M", "L"],
    "commonWords": ["sample", "product"]
  }
}
```

---

### API Errors
- If any external API errors occur, such as failure in retrieving products, error messages are logged, and an appropriate status is returned (e.g., `500 Internal Server Error`).

---

## 3. Logging

Logging is implemented using the `ILogger` interface. Logs are captured for:
- API requests (fetching products).
- Validation errors (e.g., price validation).
- Responses from external services (e.g., API call to `mocky.io`).
- Highlighting results and price filtering.

### **HTTP Client Configuration**
- The `HttpClient` used for fetching product data from external sources has a timeout configuration to prevent long-running requests.
- Timeout is set to 20 seconds to handle network latency.

---

## 4. Future Improvements
- **Caching**: Add caching to improve performance for frequently accessed data.
- **Authentication**: Introduce authentication for API security.
- **Pagination**: Implement pagination to handle large product datasets.