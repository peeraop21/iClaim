<template>
    <div class="container" align="center">
        <!--<label style="font-size:30px">ขณะนี้ระบบกำลังโหลดเอกสาร คำร้องขอรับค่าเสียหายเบื้องต้น</label>-->
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
    import mixin from '../../mixin/index.js'
    import liff from '@line/liff'
    import axios from 'axios'
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        components: {
            Loading
        },
        mixins: [mixin],
        data() {
            return {
                isLoading: true,
                data: null
            }
        },
        methods: {
            getPDF() {
                var url = this.$store.state.envUrl + '/api/Approval/DownloadPdfBoto3'
                const body = {
                    SystemId: '02',
                    TemplateId: '03',
                    DocumentId: '06',
                    RefId: this.data[3] + '|' + this.data[1] + '|' + this.data[2],
                };
                axios.post(url, JSON.stringify(body), {
                    responseType: 'arraybuffer',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: "Bearer " + this.$store.state.jwtToken.token,
                    }
                }).then((response) => {
                    let blob = new Blob([response.data], { type: 'application/pdf' }),
                        url = window.URL.createObjectURL(blob)
                    this.isLoading = false
                    if (liff.getOS() == "ios" || liff.getOS() == 'android') {
                        const link = document.createElement('a')
                        link.href = url
                        link.download = 'บต3' + '|' + this.data[1] + '|' + this.data[3] + '.pdf'
                        document.body.appendChild(link)
                        link.click()
                        window.URL.revokeObjectURL(url)
                        setTimeout(() => {
                            // For Firefox it is necessary to delay revoking the ObjectURL
                            document.body.removeChild(link);
                            window.URL.revokeObjectURL(url);
                        }, 100)
                        setTimeout(() => {
                            liff.closeWindow();
                        }, 3000)
                    } else {
                        window.open(url)
                        setTimeout(() => {
                            liff.closeWindow();
                        }, 3000)
                    }
                }).catch(function (error) {
                    alert(error);
                });
            },
            
        },
        async created() {
            this.$store.state.hasRegistered = true
            const params = new Proxy(new URLSearchParams(window.location.search), {
                get: (searchParams, prop) => searchParams.get(prop),
            });
            let value = params.pdf;
            this.data = window.atob(value).split('|')
            this.$store.state.userTokenLine = this.data[0]
            await this.getJwtToken()
            this.getPDF()
        },


    }
</script>
