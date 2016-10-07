var urlTotal = '/images/getimages'

function amountRaised(data) {
    $('.amount-raised').append('£' + data.Total + ',00')
}

function loadImages(data) {
    var imagesList = data.ImageList.map(function (item) {
        return '<li class="photos__item"><a href="#!" class="photo__link"><img src="' + item.LowResImageUrl + '" class="photo-tumb"><div class="detail-container"><button class="photo__close">X</button><img src="' + item.HighResImageUrl + '" class="photo-detail"><p class="photo__location">' + item.Location + '</p></div></a></li>';
  });
  return imagesList;
}

$(document).ready(function() {
  var API_call = $.ajax({
    url: urlTotal,
    type: 'GET',
    dataType: 'json',
  })
  .done(function(data) {
    console.log("success");
    amountRaised(data)

    $('#imagesContainer').html(loadImages(data).join(''))

    console.log();
  })
  .fail(function() {
    console.log("error");
  })
  .always(function() {
    console.log("complete");
  });
});

$('.photos').on('click', '.photo__link', function () {
    $(this).find('.detail-container').addClass('is-active');
});

$('.photos').on('click', '.photo__close', function (e) {
    e.stopPropagation()
    $(this).parent().removeClass('is-active');
});