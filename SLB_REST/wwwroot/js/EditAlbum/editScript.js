$(function () {
    var id = document.querySelector("main ul").getAttribute("id");

	var album = function () { };
	var getAlbum = new album();
	getAlbum.id = id;
	getAlbum.title = "?";
	getAlbum.styles = "?";
	getAlbum.genres = "?";
	getAlbum.images = "?";
	getAlbum.videos = "?";
	getAlbum.artists = "?";
	getAlbum.tracks = "?";
	getAlbum.extraartists = "?";

	var data = JSON.stringify(getAlbum);

    $.ajax({
        url: `/Home/GetAlbum`,
		type: "GET",
		data: { data: data },
        dataType: "json"
    }).done(function (result) {
        addPagination(id);

    }).fail(function (e) {
    })


});


function addPagination(id) {
    var tabs = document.querySelectorAll("main ul li");

    for (let i = 0; i < tabs.length; i++) {

        tabs[i].addEventListener("click", function (event) {
            $("iframe").remove();
            var tabActive = document.querySelector("main ul li a.active");
            tabActive.classList.remove("active");
            event.path[0].classList.add("active");
        });

        var a = tabs[i].querySelector("a");

        if (a.innerText == "Genres") {
            tabs[i].addEventListener("click", function () {
                editTags(id, "Genre");
            });
        }

        if (a.innerText == "Styles") {
            tabs[i].addEventListener("click", function () {
                editTags(id, "Style");
            });
        }

        if (a.innerText == "Artists") {
            tabs[i].addEventListener("click", function () {
                editTags(id, "Artist");
            });
        }

        if (a.innerText == "Images") {
            tabs[i].addEventListener("click", function () {
                editImages(id);
            });
        }

        if (a.innerText == "Videos") {
            tabs[i].addEventListener("click", function () {
                editVideos(id);
            });
        }

        if (a.innerText == "Album") {
            tabs[i].addEventListener("click", function () {
                addAlbumThumbEdit(id);
            });
        }

    }
    addAlbumThumbEdit(id);

}


