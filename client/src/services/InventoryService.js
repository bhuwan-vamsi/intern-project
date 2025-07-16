const API_URL = "http://localhost:5256/api/inventory";

async function getData() {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error("Fetch error");
    return await response.json();
  } catch (error) {
    console.error("Failed to fetch inventory data:", error);
    return null;
  }
}

async function createItem(item) {
  try {
    const response = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(item),
    });
    if (!response.ok) throw new Error("Create failed");
  } catch (error) {
    console.error("Failed to create item:", error);
  }
}

async function deleteItem(id) {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE",
    });
    if (!response.ok) throw new Error("Delete failed");
  } catch (error) {
    console.error("Failed to delete item:", error);
  }
}

async function updateItem(item) {
  try {
    const response = await fetch(`${API_URL}/${item.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(item),
    });
    if (!response.ok) throw new Error("Update failed");
  } catch (error) {
    console.error("Failed to update item:", error);
  }
}

export { getData, createItem, deleteItem, updateItem };
