<template>
    <div>
        <b-navbar class="navbar-digital-cliam justify-content-between mb-4" style="">
            <p class="b-navbar-brand mt-3">Digital Claim</p>
            <router-link to="/"><ion-icon name="home" style="font-size: 23px; color: white; margin-top: 10px;"></ion-icon></router-link>
            
        </b-navbar>
    </div>
</template>

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

<style>
    .navbar-digital-cliam {
        /*background-color: var(--main-color);*/
        background: -webkit-linear-gradient(var(--main-color), rgb(114 60 177));
        text-align: center;
        height: 50px;
        padding-left: 30px;
        padding-right: 30px;
        box-shadow: 0 1px 3px 0 rgba(0,0,0,.2);
    }
    .b-navbar-brand {
        background: -webkit-linear-gradient(#eee, #fff);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        font-size: 23px;
        font-weight: bold;
    }
</style>