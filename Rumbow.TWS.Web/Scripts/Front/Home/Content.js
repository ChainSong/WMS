function select(item) {
   var selectedStyle = ' selectedStyle';
   var span = item.children[0];
   if (select.current) {
       select.current.className = select.current.className.replace(selectedStyle, '');
   }
   span.className = span.className + selectedStyle;
   select.current = span;
}

window.onload = function () {
    var o = document.getElementsByTagName('A');
    if (o && o.length) o[0].click();
}
