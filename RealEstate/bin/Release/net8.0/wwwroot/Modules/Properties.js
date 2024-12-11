
var ClsProperties = {
    GetPropertiesForSale: function () {
        const sortOrder = document.getElementById("sortOrder").value;

        const url = `https://kareemabdelwahab-001-site1.ktempurl.com/api/Properties/GetPropertiesForSale?sortOrder=${sortOrder}`;


        Helper.AjaxCallGet(url, {}, "json",
            function (data) {

                $('#ItemPagination').pagination({
                    dataSource: data,
                    pageSize: 10,
                    callback: function (data, pagination) {
                        var htmlData = "";
                        var htmlData2 = "";

                        for (var i = 0; i < data.length; i++) {
                            htmlData += ClsProperties.DrawItem(data[i]);
                            htmlData2 += ClsProperties.DrawItem2(data[i]);

                        }
                        var d1 = document.getElementById('ItemArea1');
                        var d2 = document.getElementById('ItemArea2');

                        d1.innerHTML = htmlData;
                        d2.innerHTML = htmlData2;
                    }
                });
            }, function () { });
    },
    GetPropertiesForRent: function () {
        const sortOrder = document.getElementById("sortOrder").value;
        const url = `https://kareemabdelwahab-001-site1.ktempurl.com/api/Properties/GetPropertiesForRent?sortOrder=${sortOrder}`;

        Helper.AjaxCallGet(url, {}, "json",
            function (data) {

                $('#ItemPagination').pagination({
                    dataSource: data,
                    pageSize: 10,
                    callback: function (data, pagination) {
                        var htmlData = "";
                        var htmlData2 = "";

                        for (var i = 0; i < data.length; i++) {
                            htmlData += ClsProperties.DrawItem(data[i]);
                            htmlData2 += ClsProperties.DrawItem2(data[i]);

                        }
                        var d1 = document.getElementById('ItemArea1');
                        var d2 = document.getElementById('ItemArea2');

                        d1.innerHTML = htmlData;
                        d2.innerHTML = htmlData2;
                    }
                });
            }, function () { });
    },

    Searching: function (statusId, typeId, governorateId, cityId, bedrooms, price) {

        const sortOrder = document.getElementById("sortOrder").value;

        const url = new URL(`https://kareemabdelwahab-001-site1.ktempurl.com/api/Properties/PropertiesSearchAsync`);
        const params = {
            sortOrder,
            statusId,
            typeId,
            governorateId,
            cityId,
            bedrooms,
            price
        };

        Object.keys(params).forEach(key => {
            if (params[key] !== undefined && params[key] !== null) {
                url.searchParams.append(key, params[key]);
            }
        });

        Helper.AjaxCallGet(url, {}, "json",
            function (data) {
                var d1 = document.getElementById('ItemArea1');
                var d2 = document.getElementById('ItemArea2');
                $('#ItemPagination').pagination({
                    dataSource: data,
                    pageSize: 10,
                    callback: function (data, pagination) {
                        var htmlData = "";
                        var htmlData2 = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                htmlData += ClsProperties.DrawItem(data[i]);
                                htmlData2 += ClsProperties.DrawItem2(data[i]);

                            }
                            d1.innerHTML = htmlData;
                            d2.innerHTML = htmlData2;
                        } else {
                            d1.innerHTML = '<h2>لا توجد عقارات مطابقة للبحث</h2>';
                            d2.innerHTML = '<h2>لا توجد عقارات مطابقة للبحث</h2>';
                        }
                    }
                });
            }, function () { });
    },




    DrawItem: function (item) {
        const imageUrl = item.image || '/Uploads/DefaultImages/default-image.jpg'; // Fallback if no image URL

        var data = ` 
        <div class="col-lg-4 col-sm-6 col-12">
                <div class="ltn__product-item ltn__product-item-4 ltn__product-item-5 text-center---">
                    <div class="product-img">
                        <a href="/home/PropertyDetails/${item.propertyId}"><img src="${imageUrl}" alt="PropertyImage"></a>
                    </div>
                    <div class="product-info">
                        <div class="product-badge">
                            <ul>
                                <li class="sale-badg">${item.status}</li>
                            </ul>
                        </div>
                        <div class="product-description">
                            ${item.shortDescription}
                        </div>
                        <div class="product-img-location">
                            <ul>
                                <li>
                                    <a><i class="flaticon-pin"></i>${item.governorate}, ${item.city}</a>
                                </li>
                            </ul>
                        </div>
                        <ul class="ltn__list-item-2--- ltn__list-item-2-before--- ltn__plot-brief">
                            <li>
                                <span>${item.bedrooms}</span>

                                غرف
                            </li>
                            <li>
                              <span>${item.bathrooms}</span>
                                حمام
                            </li>
                            <li>
                                   <span>${item.area}</span>
                                المساحة
                            </li>
                        </ul>
                        <ul class="ltn__list-item-2--- ltn__list-item-2-before--- ltn__plot-brief">
                            <li>
                                تاريخ النشر
                                <span>${item.createdDate}</span>
                            </li>

                        </ul>
                        <div class="product-hover-action">
                          <div class="product-hover-action">
	                            <ul>
		                            <li>
			                            <a href="#" title="Quick View" data-bs-toggle="modal" data-bs-target="#quick_view_modal"
			                               data-property-id="${item.propertyId}"
			                               data-property-price="${item.price}"
			                               data-property-description="${item.shortDescription}"
			                               data-property-image="${imageUrl}"
			                               data-property-type="${item.type}"
			                               data-property-status="${item.status}">
				                            <i class="flaticon-expand"></i>
			                            </a>
		                            </li>
		                            <li>
			                            <a href="#" title="Wishlist" data-bs-toggle="modal" data-bs-target="#liton_wishlist_modal"
			                               data-id="${item.propertyId}"
			                               data-image="${imageUrl}"
			                               data-price="${item.price}"
			                               data-status="${item.status}"
			                               data-type="${item.type}"
			                               onclick="addToWishlist(this.dataset.id,this.dataset.image,this.dataset.price,this.dataset.status,this.dataset.type)">
				                            <i class="flaticon-heart-1"></i>
			                            </a>
		                            </li>
		                            <li>
			                            <a href="/home/PropertyDetails/${item.propertyId}" title="Product Details">
				                            <i class="flaticon-add"></i>
			                            </a>
		                            </li>
	                            </ul>
                          </div>

                        </div>
                    </div>
                    <div class="product-info-bottom">
                        <div class="product-price">
                            <span>${item.price}  <label>/جنية</label></span>
                        </div>
                    </div>
                </div>
        </div>
            `;
        return data;
    },

    DrawItem2: function (item) {
        const imageUrl = item.image || '/Uploads/DefaultImages/default-image.jpg'; // Fallback if no image URL

        var data = ` 
        <div class="col-lg-12">
            <div class="ltn__product-item ltn__product-item-4 ltn__product-item-5">
                <div class="product-img">
                    <a href="/home/PropertyDetails/${item.propertyId}"><img src="${imageUrl}" alt="PropertyImage"></a>
                </div>
                <div class="product-info">
                    <div class="product-badge-price">
                        <div class="product-badge">
                            <ul>
                                <li class="sale-badg">${item.status}</li>
                            </ul>
                        </div>
                        <div class="product-price">
                            <span>${item.price}  <label>/جنية</label></span>
                        </div>
                    </div>
            
                    <div class="product-description">
                            ${item.shortDescription}
                    </div>
                    <div class="product-img-location">
                        <ul>
                            <li>
                                <a href="locations.html"><i class="flaticon-pin"></i> ${item.governorate}, ${item.city}</a>
                            </li>
                        </ul>
                    </div>
                    <ul class="ltn__list-item-2--- ltn__list-item-2-before--- ltn__plot-brief">

                        <li>
                            <span>${item.bedrooms}</span>

                            غرف
                        </li>
                        <li>
                            <span>${item.bathrooms}</span>
                            حمام
                        </li>
                        <li>
                            <span>${item.area}</span>
                            المساحة
                        </li>
                    </ul>

                    <ul class="ltn__list-item-2--- ltn__list-item-2-before--- ltn__plot-brief">

                        <li>
                            تاريخ النيشر
                            <span>${item.createdDate}</span>
                        </li>

                    </ul>
                </div>
                <div class="product-info-bottom">
                   <div class="product-hover-action">
	                    <ul>
		                    <li>
			                    <a href="#" title="Quick View" data-bs-toggle="modal" data-bs-target="#quick_view_modal"
			                       data-property-id="${item.propertyId}"
			                       data-property-price="${item.price}"
			                       data-property-description="${item.shortDescription}"
			                       data-property-image="${imageUrl}"
			                       data-property-type="${item.type}"
			                       data-property-status="${item.status}">
				                    <i class="flaticon-expand"></i>
			                    </a>
		                    </li>
		                    <li>
			                    <a href="#" title="Wishlist" data-bs-toggle="modal" data-bs-target="#liton_wishlist_modal"
			                       data-id="${item.propertyId}"
			                       data-image="${imageUrl}"
			                       data-price="${item.price}"
			                       data-status="${item.status}"
			                       data-type="${item.type}"
			                       onclick="addToWishlist(this.dataset.id,this.dataset.image,this.dataset.price,this.dataset.status,this.dataset.type)">
				                    <i class="flaticon-heart-1"></i>
			                    </a>
		                    </li>
		                    <li>
			                    <a href="/home/PropertyDetails/${item.propertyId}" title="Product Details">
				                    <i class="flaticon-add"></i>
			                    </a>
		                    </li>
	                    </ul>
                    </div>
                </div>
            </div>
        </div>
            `;
        return data;
    },

    GetAllCitiesForRent: function () {
        const governorateId = document.getElementById("governorate").value;

        const url = `https://kareemabdelwahab-001-site1.ktempurl.com/api/Properties/GetCities?governorateId=${governorateId}`;

        Helper.AjaxCallGet(url, {}, "json",
            function (data) {
                const citySelect = document.getElementById("city");

                // Clear all existing options
                citySelect.innerHTML = '';

                // Add a default option
                const defaultOption = document.createElement('option');
                defaultOption.textContent = '-- المدينة--';
                defaultOption.value = null;
                citySelect.appendChild(defaultOption);

                // Add new options
                data.forEach(city => {
                    const option = document.createElement('option');
                    option.textContent = city.cityName; // Displayed text
                    option.value = city.cityId;        // Option value
                    citySelect.appendChild(option);
                });

               
                $('.nice-select').niceSelect('update');

            }, function () { });
    },

    GetAllCitiesForSale: function () {
        const governorateId = document.getElementById("governorate2").value;
        const url = `https://kareemabdelwahab-001-site1.ktempurl.com/api/Properties/GetCities?governorateId=${governorateId}`;
        Helper.AjaxCallGet(url, {}, "json",
            function (data) {
                const citySelect = document.getElementById("city2");

                // Clear all existing options
                citySelect.innerHTML = '';

                // Add a default option
                const defaultOption = document.createElement('option');
                defaultOption.textContent = '-- المدينة--';
                defaultOption.value = null;
                citySelect.appendChild(defaultOption);

                // Add new options
                data.forEach(city => {
                    const option = document.createElement('option');
                    option.textContent = city.cityName; // Displayed text
                    option.value = city.cityId;        // Option value
                    citySelect.appendChild(option);
                });


                $('.nice-select').niceSelect('update');

            }, function () { });
    },




}

