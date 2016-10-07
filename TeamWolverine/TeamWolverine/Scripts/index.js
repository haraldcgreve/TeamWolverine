var urlTotal = '/images/getimages'

function amountRaised(data) {
    $('.amount-raised').append('£' + data.Total + ',00')
}

function loadImages(data) {
    var imagesList = data.ImageList.map(function (item) {
        return '<li class="photos__item"><a href="#!" class="photo__link"><button class="photo__close">X</button><img src="' + item.LowResImageUrl + '" class="photo-tumb"><img src="' + item.HighResImageUrl + '" class="photo-detail"></a></li>';
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

$('.photos').on('click', '.photo__link', function() {
  $(this).find('.photo-detail').addClass('is-active');
  $(this).find('.photo__close').addClass('is-active');
});

$('.photos').on('click', '.photo__close', function(e) {
  e.stopPropagation()
  $(this).removeClass('is-active');
  $(this).parent().find('.photo-detail').removeClass('is-active');
});
