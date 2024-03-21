$(() => {
    setInterval(() => {
        $.get("/Home/GetImageLikes", { id: $('img').data('image-id') }, (likesCount) => {
            $('#likes-count').text(likesCount);
        })
    }, 1000);

    $('button').on('click', function () {
        console.log('clicked');
        $(this).prop('disabled', true);
        $.post("/Home/IncrementImagesLikes", { id: $('img').data('image-id') });
    })
})