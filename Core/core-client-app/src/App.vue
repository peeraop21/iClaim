<template>
    <div id="app">

        <nav-menu></nav-menu>

        <router-view />


    </div>
</template>

<script>

    import NavMenu from './components/_Layout/NavMenu.vue'



    export default {
        components: {
            NavMenu
        },

        name: 'App',
        data() {
            return {

            }
        },
        methods: {


        },
        mounted() {


        },
        created() {
            

            //liff.getProfile().then(profile => {
            //    const getProfile = profile.userId;
            //    alert(getProfile);
            //});

            //await liff.init({
            //    liffId: '1656525617-BqQZ3o1z',

            //    /*withLoginOnExternalBrowser: false,*/
            //}).then(() => {
            //    if (liff.isLoggedIn()) {
            //        liff.getProfile().then(profile => {
            //            this.$store.state.userTokenLine = profile.userId
            //            this.show = false
            //            alert(profile.displayName);
            //        }).catch(err => alert(err));
            //    } else {
            //        liff.login();
            //    }

            //    //const getContext = liff.getContext();
            //    //this.$store.state.userTokenLine = getContext.userId
            //    //alert(getContext.userTokenLine);

            //}).catch(err => {
            //    alert(err);
            //    throw err
            //});


            /*this.$store.state.userTokenLine = "Uf3f96dd0506eec532162b377d7c0nioro";*/

        }


    }
</script>
<style>
    #app {
        font-family: Mitr;
        font-size: 13px;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    :root {
        --main-color: #5c2e91;
    }


    #nav {
        padding: 30px;
    }

        #nav a {
            font-weight: bold;
            color: #2c3e50;
        }

            #nav a.router-link-exact-active {
                color: #42b983;
            }
</style>
