<template>
    <header>
        <ul>
            <li style="float:left"><a  href="\">Digital Claim</a></li>
            <!--<li style="float:right"><a href="#about"><ion-icon name="chevron-down-outline"></ion-icon></a></li>-->
        </ul>
    </header>
</template>


<style>
a.navbar-brand {
    white-space: normal;
    text-align: center;
    word-break: break-all;
}

html {
    font-size: 14px;
}

@media (min-width: 768px) {
    html {
        font-size: 16px;
    }
}

.box-shadow {
    box-shadow: 0 .25rem .75rem rgba(0, 0, 0, .05);
}

ul {
  list-style-type: none;
  margin: 0;
  padding: 0px 20px;
  overflow: hidden;
  background-color: #333;
  font-size: 25px;
  font-weight: bold;
  background-color: #5c2e91;
  /*box-shadow: 10px 10px #dddddd;*/
}

li {
  float: left;
}

li a {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}

li a:hover:not(.active) {
  text-decoration: none;
  color: #dddddd;
}


</style>
<script>
    export default {
        name: "NavMenu",
        data() {
            return {
                isExpanded: false
            }
        },
        methods: {
            collapse() {
                this.isExpanded = false;
            },

            toggle() {
                this.isExpanded = !this.isExpanded;
            }
        }
    }
</script>