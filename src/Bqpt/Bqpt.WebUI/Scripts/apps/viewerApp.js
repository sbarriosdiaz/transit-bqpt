var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 5,
    zoomRange = 0.20,
    canvas = document.getElementById('the-canvas'),
    ctx = canvas.getContext('2d');

function renderPage(num, scale) {
    pageRendering = true;
    // Using promise to fetch the page
    pdfDoc.getPage(num).then(function (page) {
        var viewport = page.getViewport(scale);
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRendering = false;
            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });
    $('#page_count').removeClass('hidden');
    // Update page counters
    document.getElementById('page_num').value = num;
}

function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num, scale);
    }
}

function onPrevPage() {
    if (pageNum <= 1) {
        return;
    }
    pageNum--;
    var scale = pdfDoc.scale;
    queueRenderPage(pageNum, scale);
}
document.getElementById('prev').addEventListener('click', onPrevPage);

/**
 * Displays next page.
 */
function onNextPage() {
    if (pageNum >= pdfDoc.numPages) {
        return;
    }
    pageNum++;
    var scale = pdfDoc.scale;
    queueRenderPage(pageNum, scale);
}

document.getElementById('next').addEventListener('click', onNextPage);

PDFJS.getDocument(url).then(function (pdfDoc_) {
    pdfDoc = pdfDoc_;
    var documentPagesNumber = pdfDoc.numPages;
    document.getElementById('page_count').textContent = '/ ' + documentPagesNumber;

    $('#page_num').on('change', function () {
        var pageNumber = Number($(this).val());

        if (pageNumber > 0 && pageNumber <= documentPagesNumber) {
            queueRenderPage(pageNumber, scale);
        }
    });

    renderPage(pageNum, scale);
});