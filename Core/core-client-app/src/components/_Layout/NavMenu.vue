<template>
    <div>
        <b-navbar class="navbar-digital-cliam justify-content-between mb-4" style="">
            <div align="left" style="width: 100%;">
                <ion-icon name="chevron-back-outline" align="left" style=" margin-bottom: -4px; padding-right: 20px; font-size: 23px; color: white;" @click="onClickBackBtn"></ion-icon>
                <label align="left" class="b-navbar-brand mt-1">Digital Claim</label>
            </div>
            <!--<router-link to="/"><ion-icon name="chevron-back-outline" style="font-size: 23px; color: white; margin-top: 10px;"></ion-icon></router-link>
            <p class="b-navbar-brand mt-3">Digital Claim</p>-->
            <router-link to="/"><ion-icon name="home" style="font-size: 23px; color: white; margin-top: 10px;"></ion-icon></router-link>

        </b-navbar>
    </div>
</template>

<script>
    export default {
        name: "NavMenu",
        data() {
            return {
                isExpanded: false,
                
            }
        },
        methods: {
            collapse() {
                this.isExpanded = false;
            },
            onClickBackBtn() {
                var routeName = this.$route.name
                if (routeName == "Accident") {
                    this.$router.push({ name: 'Advice' })
                } else if (routeName == "Rights") {
                    this.$router.push({ name: 'Accident' })
                } else if (routeName == "RightsHistory") {
                    this.$router.push({ name: 'Rights' })
                } else if (routeName == "RightsHistoryDetail") {
                    this.$router.push({ name: 'RightsHistory', params: { id: this.$route.params.id }  })
                } else if (routeName == "Claim") {
                    this.$router.push({ name: 'Rights' })   
                } else if (routeName == "ConfirmOTP") {
                    this.$router.push({ name: 'Claim', params: { id: this.$route.params.id }})
                } else if (routeName == "CheckStatus") {
                    this.$router.push({ name: 'Accident' })
                } else if (routeName == "ConfirmMoney") {
                    this.$router.push({ name: 'CheckStatus', params: { id: this.$route.params.id } })
                } else if (routeName == "ClaimDetail") {
                    this.$router.push({ name: 'CheckStatus', params: { id: this.$route.params.id } })
                } else if (routeName == "Ocr") {
                    this.$router.push({ name: 'Advice' })
                } else if (routeName == "Rating") {
                    this.$router.push({ name: 'CheckStatus', params: { id: this.$route.params.id } })
                } else if (routeName == "AddDocument") {
                    this.$router.push({ name: 'CheckStatus', params: { id: this.$route.params.id } })
                }
            },

            toggle() {
                this.isExpanded = !this.isExpanded;
            }
        }
    }
</script>

<style>
    .navbar-digital-cliam {
        background-color: var(--main-color);
        /*background: -webkit-linear-gradient(var(--main-color), rgb(114 60 177));*/
        text-align: center;
        height: 50px;
        padding-left: 15px;
        padding-right: 20px;
        /*box-shadow: 0 1px 3px 0 rgba(0,0,0,.2);*/
    }
    .b-navbar-brand {
        background-color: white;
        /*background: linear-gradient(45deg, #c331ff, var(--main-color));*/
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        font-size: 23px;
        font-weight: bold;
    }
</style>