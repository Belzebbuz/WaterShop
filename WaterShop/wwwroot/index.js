$(document).ready(function () {
  fillData();
  fillBalance();
  $("#1rub").click(function () {
    addMoneyValue(1);
  });
  $("#2rub").click(function () {
    addMoneyValue(2);
  });
  $("#5rub").click(function () {
    addMoneyValue(5);
  });
  $("#10rub").click(function () {
    addMoneyValue(10);
  });
  $("#giveChange").click(function () {
    giveChange();
  });
});
const fillBalance = () => {
  $.ajax({
    url: "/api/balance",
    method: "GET",
    success: function (data) {
      $("#balanceValue").text(data);
    },
    error: function () {},
  });
};
const fillData = () => {
  $.ajax({
    url: "/api/water",
    method: "GET",
    success: function (data) {
      $("#dataContainer").empty();
      data.waters.forEach((element) => {
        const cardItem = $(
          `<div class="col" style="width:20rem;" id="${element.id}">`
        ).append(textCardItem(element, data.balance));
        cardItem.find(".btn-buy").click(function () {
          buyWater(element.id);
        });
        $("#dataContainer").append(cardItem);
      });
    },
    error: function () {},
  });
};
const addMoneyValue = (value) => {
  $.ajax({
    url: "/api/balance",
    method: "POST",
    contentType: "application/json",
    data: JSON.stringify(value),
    success: function (data) {
      $("#monets").empty();
      $("#balanceValue").text(data);
      fillData();
    },
    error: function (error) {
      Swal.fire({
        icon: "error",
        title: "Ошибка...",
        text: error.responseText,
        footer: '<a href="">Why do I have this issue?</a>',
      });
    },
  });
};

const buyWater = (id) => {
  $.ajax({
    url: `api/water/${id}/buy`,
    method: "GET",
    success: function () {
      updateWaterCard(id);
    },
    error: function (error) {
      Swal.fire({
        icon: "error",
        title: "Ошибка...",
        text: error.responseText,
        footer: '<a href="">Why do I have this issue?</a>',
      });
    },
  });
};

const giveChange = () => {
  $.ajax({
    url: "api/balance/change",
    method: "GET",
    success: function (data) {
      console.log(data);
      $("#balanceValue").text(data.balance);
      $("#monets").empty();
      $("#monets").text(`Сдача: ${data.monets.map((x) => `${x}р. `)}`);
    },
    error: function (error) {
      Swal.fire({
        icon: "error",
        title: "Ошибка...",
        text: error.responseText,
        footer: '<a href="">Why do I have this issue?</a>',
      });
    },
  });
};
const updateWaterCard = (id) => {
  $.ajax({
    url: `api/water/${id}`,
    method: "GET",
    success: function (data) {
      $(`#${id}`).empty();
      var cardItem = $(`#${id}`).append(textCardItem(data.water, data.balance));
      $("#balanceValue").text(data.balance);
      cardItem.find(".btn-buy").click(function () {
        buyWater(data.water.id);
      });
    },
    error: function (error) {
      showError(error.responseText);
    },
  });
};
function textCardItem(element, balance) {
  return `
  <div class="card">
    <img src="${element.imagePath}" class="card-img-top p-4" />
    <div class="card-body">
      <h4 class="card-title">${element.title}</h4>
      <h5 class="card-text">Цена: ${element.cost}</h5>
      <h5 class="card-text">Осталось: ${element.count}</h5>
      <button class="btn btn-primary btn-buy" ${
        element.count === 0 || balance < element.cost ? "disabled" : ""
      }>Купить</button>
    </div>
  </div>
  `;
}
