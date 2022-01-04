<template>
    <div class="container" align="center">
        <label style="font-size:30px">ขณะนี้ระบบกำลังโหลดเอกสาร คำร้องขอรับค่าเสียหายเบื้องต้น</label>
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>


    </div>
</template>

<style>
</style>

<script>
    import liff from '@line/liff'
    import axios from 'axios'
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        components: {
            Loading
        },
        data() {
            return {
                isLoading: true,

            }
        },
        methods: {
            getPDF() {
                var url = this.$store.state.envUrl + '/api/genpdf';
                const body = {
                    AccNo: this.$route.params.accNo,
                    VictimNo: parseInt(this.$route.params.victimNo),
                    AppNo: parseInt(this.$route.params.appNo),
                    Channel: 'HOSPITAL',
                    UserIdCard: this.$route.params.userIdCard
                };
                
                axios.post(url, JSON.stringify(body), {
                    responseType: 'arraybuffer',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                    .then((response) => {
                        let blob = new Blob([response.data], { type: 'application/pdf' }),
                            url = window.URL.createObjectURL(blob)
                        this.isLoading = false
                        liff.openWindow({
                            url: url
                        });
                        setTimeout(() => {
                            liff.closeWindow();
                        }, 3000)
                    })
                    .catch(function (error) {
                        alert(error);
                    });

            }

        },
        mounted() {
            this.getPDF();

        }

    }
</script>
