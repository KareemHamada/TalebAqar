$('#quick_view_modal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var propertyId = button.data('property-id');
    var propertyType = button.data('property-type');
    var propertyPrice = button.data('property-price');
    var propertyDescription = button.data('property-description');
    var propertyImage = button.data('property-image');
    var propertyStatus = button.data('property-status');
    var propertyCurrency = button.data('property-currency');

    // Update modal content
    $('#modal-product-type').text(propertyType);


    // Determine the class to add based on the value of propertyStatus
    var statusClass = propertyStatus === "للبيع" ? "sale-badge bg-green--- p-1" : "sale-badge bg-green p-1";
    $('#modal-product-status').text(propertyStatus);
    $('#modal-product-price').text(propertyPrice);
    $('#modal-product-currency').text(propertyCurrency);

    $('#modal-product-image').attr('src', propertyImage);
    $('#modal-product-link').attr('href', "/home/PropertyDetails/" + propertyId);
    $('#modal-product-id').text(propertyId);
    // Render the HTML inside the modal

    $('#modal-product-description').html(propertyDescription);
});


function addItemToWishlist() {
    const propertyId = document.getElementById("modal-product-id").innerHTML;
    const imageUrl = document.getElementById("modal-product-image").src;
    const price = document.getElementById("modal-product-price").innerHTML;
    const status = document.getElementById("modal-product-status").innerHTML;
    const type = document.getElementById("modal-product-type").innerHTML;
    const currency = document.getElementById("modal-product-currency").innerHTML;

    addToWishlist(propertyId, imageUrl, price, status, type, currency);
}
function addToWishlist(propertyId, imageUrl, price, status, type, currency) {

    // Retrieve current wishlist from local storage
    let wishlist = JSON.parse(localStorage.getItem("wishlist")) || [];

    // Check if the property is already in the wishlist
    if (!wishlist.some(item => item.id === propertyId)) {
        wishlist.push({
            id: propertyId,
            image: imageUrl,
            price: price,
            status: status,
            type: type,
            currency: currency
        });

        // Save updated wishlist back to local storage
        localStorage.setItem("wishlist", JSON.stringify(wishlist));
        updateWishlistCount();
        // Show modal after adding
        showWishlistModal(imageUrl, "تمت الإضافة إلى المفضلة");
    } else {
        // Show modal if exist
        showWishlistModal(imageUrl, "تمت الإضافة إلى المفضلة سابقا");
    }
}

// Load Wishlist and Render Table
function loadWishlist() {
    let wishlist = JSON.parse(localStorage.getItem("wishlist")) || [];
    let tableBody = document.querySelector(".shoping-cart-table tbody");
    let noElements = document.getElementById("no-element");

    tableBody.innerHTML = ""; // Clear existing rows
    if (wishlist.length > 0) {
       
        wishlist.forEach(item => {
            let row = `
                    <tr>
                        <td class="cart-product-remove"><a onclick="removeFromWishlist(${item.id})">x</a></td>
                        <td class="cart-product-image"><a href="/home/propertyDetails/${item.id}"><img src="${item.image}" alt="image"></a></td>
                        <td class="cart-product-type">${item.type} </td>
                        <td class="cart-product-status">${item.status} </td>
                        <td class="cart-product-price">${item.price} / ${item.currency}</td>
                    </tr>
                `;
            tableBody.innerHTML += row;
        });
    } else {
        noElements.innerHTML = '<h2>لا توجد عقارات فى المفضلة</h2>';
    }
}

// Remove from Wishlist
function removeFromWishlist(propertyId) {

    let wishlist = JSON.parse(localStorage.getItem("wishlist")) || [];

    // Convert propertyId to match type (number or string)
    wishlist = wishlist.filter(item => parseInt(item.id) !== propertyId && parseInt(item.id) !== parseInt(propertyId));

    localStorage.setItem("wishlist", JSON.stringify(wishlist));
    updateWishlistCount();
    loadWishlist(); // Refresh table
}

function showWishlistModal(imageUrl, message) {

    document.getElementById("messageAdding").innerHTML = `<i class="fa fa-check-circle"></i> ${message}`;
    // Populate modal content
    document.getElementById("wishlistModalImage").src = imageUrl || "~/Uploads/DefaultImages/default-image.png";

}


function updateWishlistCount() {
    const wishlist = JSON.parse(localStorage.getItem("wishlist")) || [];
    const wishlistCountElements = document.querySelectorAll(".mini-cart-icon sup");

    // // Update the count in the <sup> element
    // wishlistCountElement.textContent = wishlist.length;

    // Update the count for each <sup> element
    wishlistCountElements.forEach(element => {
        element.textContent = wishlist.length;
    });
}

// Initialize wishlist count on page load
document.addEventListener("DOMContentLoaded", updateWishlistCount);
