import React, { useEffect, useState } from 'react';
import {
  getData,
  deleteItem,
  updateItem,
  createItem,
} from '../services/InventoryService';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import './InventoryTable.css';

const InventoryTable = () => {
  const [items, setItems] = useState([]);
  const [editItem, setEditItem] = useState(null);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [newItem, setNewItem] = useState({ itemname: '', price: '', quantity: '' });

  useEffect(() => {
    loadItems();
  }, []);

  const loadItems = async () => {
    const data = await getData();
    if (data) setItems(data);
  };

  const handleDelete = async (id) => {
    await deleteItem(id);
    loadItems();
  };

  const handleEditClick = (item) => setEditItem({ ...item });

  const handleUpdateSubmit = async (e) => {
    e.preventDefault();
    await updateItem(editItem);
    setEditItem(null);
    loadItems();
  };

  const handleCreateSubmit = async (e) => {
    e.preventDefault();
    await createItem({ ...newItem, price: parseFloat(newItem.price), quantity: parseInt(newItem.quantity) });
    setNewItem({ itemname: '', price: '', quantity: '' });
    setShowCreateForm(false);
    loadItems();
  };

  return (
    <div className="container mt-4 container-box">
      <h2 className="inventory-heading">Inventory List</h2>

      <div className="d-flex justify-content-between align-items-center mb-3">
        <button
          className="btn btn-primary"
          onClick={() => setShowCreateForm(!showCreateForm)}
        >
          {showCreateForm ? 'Cancel' : 'Create New Item'}
        </button>
      </div>

      {showCreateForm && (
        <form className="mb-4" onSubmit={handleCreateSubmit}>
          <div className="row g-2">
            <div className="col-md-4">
              <input
                type="text"
                className="form-control"
                placeholder="Item Name"
                value={newItem.itemname}
                onChange={(e) => setNewItem({ ...newItem, itemname: e.target.value })}
                required
              />
            </div>
            <div className="col-md-3">
              <input
                type="number"
                className="form-control"
                placeholder="Price"
                value={newItem.price}
                onChange={(e) => setNewItem({ ...newItem, price: e.target.value })}
                required
              />
            </div>
            <div className="col-md-3">
              <input
                type="number"
                className="form-control"
                placeholder="Quantity"
                value={newItem.quantity}
                onChange={(e) => setNewItem({ ...newItem, quantity: e.target.value })}
                required
              />
            </div>
            <div className="col-md-2">
              <button type="submit" className="btn btn-success w-100">
                Add Item
              </button>
            </div>
          </div>
        </form>
      )}

      <table className="table table-bordered inventory-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Item Name</th>
            <th>Price</th>
            <th>Quantity</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {items.length > 0 ? (
            items.map((item) => (
              <tr key={item.id} className="inventory-row">
                {editItem?.id === item.id ? (
                  <>
                    <td>{item.id}</td>
                    <td>
                      <input
                        type="text"
                        name="itemname"
                        value={editItem.itemname}
                        onChange={(e) =>
                          setEditItem({ ...editItem, itemname: e.target.value })
                        }
                        className="form-control"
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        name="price"
                        value={editItem.price}
                        onChange={(e) =>
                          setEditItem({ ...editItem, price: e.target.value })
                        }
                        className="form-control"
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        name="quantity"
                        value={editItem.quantity}
                        onChange={(e) =>
                          setEditItem({ ...editItem, quantity: e.target.value })
                        }
                        className="form-control"
                      />
                    </td>
                    <td>
                      <button
                        className="btn btn-sm btn-success me-2"
                        onClick={handleUpdateSubmit}
                      >
                        Save
                      </button>
                      <button
                        className="btn btn-sm btn-secondary"
                        onClick={() => setEditItem(null)}
                      >
                        Cancel
                      </button>
                    </td>
                  </>
                ) : (
                  <>
                    <td>{item.id}</td>
                    <td>{item.itemname}</td>
                    <td>${item.price}</td>
                    <td>{item.quantity}</td>
                    <td className="action-buttons">
                      <button
                        className="btn btn-sm btn-outline-primary me-2"
                        onClick={() => handleEditClick(item)}
                      >
                        <i className="bi bi-pencil"></i>
                      </button>
                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDelete(item.id)}
                      >
                        <i className="bi bi-trash"></i>
                      </button>
                    </td>
                  </>
                )}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="5" className="no-items">
                No items found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default InventoryTable;
