var isCreateForm = false;
$(document).ready(function () {
  fillData();
  $("#btn-create").click(function () {
    $("#costInput").val(0);
    $("#titleInput").val("");
    $("#imageInput").val("");
    $("#waterModal").modal("show");
    $("#waterForm").off("submit");
    $("#waterForm").submit(function (event) {
      event.preventDefault();
      var form = $(this)[0];
      var formData = new FormData(form);
      isCreateForm = true;
      sendWaterForm(formData);
    });
  });
  $("#btn-import").click(function () {
    $("#importModal").modal("show");
    $("#importForm").submit(function (event) {
      event.preventDefault();
      var form = $(this)[0];
      var formData = new FormData(form);
      sendImport(formData);
    });
  });
});
const fillData = () => {
  $.ajax({
    url: "/api/water",
    method: "GET",
    success: function (data) {
      $("#dataContainer").empty();
      data.waters.forEach((element) => {
        var cardItem = $(
          `<div class="col" style="width:20rem;" id="${element.id}">`
        ).append(textCardItem(element));
        subscribeToCardEvents(cardItem, element);
        $("#dataContainer").append(cardItem);
      });
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};

const sendWaterForm = (formData, id) => {
  if (isCreateForm) {
    createWater(formData);
    $("#waterModal").modal("toggle");
    return;
  }
  if (id === undefined) return;
  updateWater(id, formData);
  $("#waterModal").modal("toggle");
};

function updateWater(id, formData) {
  $.ajax({
    url: `api/water/${id}`,
    method: "PUT",
    data: formData,
    processData: false,
    contentType: false,
    success: function () {
      updateWaterCard(id);
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
}

function subscribeToCardEvents(cardItem, element) {
  cardItem.find(".btn-add").click(function () {
    addWater(element.id);
  });
  cardItem.find(".btn-delete").click(function () {
    deleteWater(element.id);
  });
  cardItem.find(".btn-remove").click(function () {
    removeWater(element.id);
  });
  cardItem.find(".btn-update").click(function () {
    $("#costInput").val(element.cost);
    $("#titleInput").val(element.title);
    $("#imageInput").val("");
    $("#waterModal").modal("show");
    $("#waterForm").off("submit");
    $("#waterForm").submit(function (event) {
      isCreateForm = false;
      event.preventDefault();
      var form = $(this)[0];
      var formData = new FormData(form);
      sendWaterForm(formData, element.id);
    });
  });
}
const addWater = (waterId) => {
  $.ajax({
    url: "api/water/add-count",
    method: "POST",
    contentType: "application/json",
    data: JSON.stringify({ id: waterId, count: 1 }),
    success: function () {
      updateWaterCard(waterId);
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
function createWater(formData) {
  $.ajax({
    url: "api/water",
    type: "POST",
    data: formData,
    processData: false,
    contentType: false,
    success: function () {
      fillData();
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
}
const deleteWater = (id) => {
  $.ajax({
    url: `api/water/${id}`,
    method: "DELETE",
    success: function () {
      fillData();
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
const updateWaterCard = (id) => {
  $.ajax({
    url: `api/water/${id}`,
    method: "GET",
    success: function (data) {
      $(`#${id}`).empty();
      var cardItem = $(`#${id}`).append(textCardItem(data.water));
      subscribeToCardEvents(cardItem, data.water);
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
const removeWater = (waterId) => {
  $.ajax({
    url: "api/water/remove-count",
    method: "POST",
    contentType: "application/json",
    data: JSON.stringify({ id: waterId, count: 1 }),
    success: function () {
      updateWaterCard(waterId);
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
const sendImport = (formData) => {
  $.ajax({
    url: "api/water/import",
    method: "POST",
    data: formData,
    processData: false,
    contentType: false,
    success: function () {
      fillData();
      $("#importModal").modal("toggle");
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
function showError(errorMessage) {
  Swal.fire({
    icon: "error",
    title: "Ошибка...",
    text: errorMessage,
    footer: '<a href="">Произошла непредвиденная ошибка, попробуйте позже</a>',
  });
}
function textCardItem(element) {
  return `
    <div class="card">
      <img src="${element.imagePath}" alt="no image" class="card-img-top p-4" />
      <div class="card-body">
        <h4 class="card-title">${element.title}</h4>
        <h5 class="card-text">Цена: ${element.cost}</h5>
        <div class="d-flex justify-content-between"> 
          <h5 class="card-text">Осталось: ${element.count}</h5>
          <div class="d-flex gap-2">
            <button style="width:40px;" class="btn btn-outline-primary btn-sm btn-add">+</button>
            <button style="width:40px" class="btn btn-outline-secondary btn-sm btn-remove" ${
              element.count === 0 ? "disabled" : ""
            }>-</button>
          </div>
        </div>
          <div class="mt-2 d-flex justify-content-between">
              <button class="btn btn-primary btn-update">Изменить</button>
              <button class="btn btn-danger btn-delete">Удалить</button>
          </div>
      </div>
    </div>
    `;
}
