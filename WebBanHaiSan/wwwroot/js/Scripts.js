function showOrderDetails(fullName, phone, address, orderDate, totalAmount) {
    document.getElementById("modalFullName").textContent = fullName;
    document.getElementById("modalPhone").textContent = phone;
    document.getElementById("modalAddress").textContent = address;
    document.getElementById("modalOrderDate").textContent = orderDate;

    // Tính toán Ship Date (Order Date + 3 ngày)
    let orderDateObj = new Date(orderDate);
    let shipDateObj = new Date(orderDateObj);
    shipDateObj.setDate(orderDateObj.getDate() + 3);
    document.getElementById("modalShipDate").textContent = shipDateObj.toISOString().split('T')[0];

    document.getElementById("modalTotalAmount").textContent = totalAmount;

    document.getElementById("orderModal").style.display = "block";
}

function closeModal() {
    document.getElementById("orderModal").style.display = "none";
}

// Đóng modal khi người dùng nhấp ra ngoài modal
window.onclick = function (event) {
    let modal = document.getElementById("orderModal");
    if (event.target == modal) {
        modal.style.display = "none";
    }
}