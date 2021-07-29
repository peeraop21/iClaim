<template>
    <header>
        <ul>
            <li style="float:left"><a  href="\">Digital Claim</a></li>
            <!--<li style="float:right"><a href="#about"><ion-icon name="chevron-down-outline"></ion-icon></a></li>-->
        </ul>
        <!--<div class="mobile-container">
            <div class="topnav">
                <a href="\" class="active">Digital Claim</a>
                <ion-icon name="chevron-down-outline"></ion-icon>
            </div>
        </div> -->
        <!-- <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand">Digital Claim</a>
                <button class="navbar-toggler"
                        type="button"
                        data-toggle="collapse"
                        data-target=".navbar-collapse"
                        aria-label="Toggle navigation"
                        @click="toggle">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse"
                     v-bind:class="{show: isExpanded}">
                    <ul class="navbar-nav flex-grow">
                       <li class="nav-item">
                            <router-link :to="{ name: 'Home' }" class="nav-link text-dark">Home</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link :to="{ name: 'Counter' }" class="nav-link text-dark">Counter</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link :to="{ name: 'FetchData' }" class="nav-link text-dark">Fetch Data</router-link>
                        </li>

                    </ul>
                </div>
            </div>
        </nav> -->
    </header>
</template>


<style>
a.navbar-brand {
    font-family: "Mitr";
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

/*.mobile-container {
  max-width: 100%;
  margin: auto;
  background-color: #555;
  color: white;
  border-radius: 10px;
}

.topnav {
  overflow: hidden;
  background-color: #333;
  position: relative;
}

.topnav a {
  color: white;
  padding: 14px 50px;
  text-decoration: none;
  font-size: 17px;
  display: block;
  font-size: 20px;
  font-family: "Mitr";
}

.active {
  background-color: #5c2e91;
  color: white;
  box-shadow: 0 .25rem .75rem rgba(0, 0, 0, .05);
}*/

ul {
  list-style-type: none;
  margin: 0;
  padding: 8px 20px;
  overflow: hidden;
  background-color: #333;
  font-family: mitr;
  font-size: 22px;
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