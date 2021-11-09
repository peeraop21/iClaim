<template>
    <div class="container" align="center">

       
        
    </div>
</template>

<style>
   
</style>

<script>
    import liff from '@line/liff'
    import axios from 'axios'

    export default {
        data() {
            return {
                
            }
        },
        methods: {
            getPDF() {

                /*var url = '/api/genpdf/GetBoto3/{accNo}/{victimNo}/{appNo}/{channel}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', appNo).replace('{channel}', this.accData.channel);*/
                var url = '/api/genpdf';
                const body = {
                    AccNo: this.$route.params.accNo,
                    VictimNo: parseInt(this.$route.params.victimNo),
                    AppNo: parseInt(this.$route.params.appNo),
                    Channel: 'HOSPITAL',
                    UserIdCard: this.$route.params.userIdCard
                };
                console.log(body)
                this.$swal({
                    title: 'กำลังโหลด',
                    html: 'ขณะนี้ระบบกำลังโหลดเอกสาร คำร้องขอรับค่าเสียหายเบื้องต้น',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                axios.post(url, JSON.stringify(body), {
                    responseType: 'arraybuffer',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                    .then((response) => {
                        console.log(response)
                        let blob = new Blob([response.data], { type: 'application/pdf' }),
                            url = window.URL.createObjectURL(blob)
                        this.$swal.close();
                        //this.$router.push(url)
                        //window.location.href = url;
                        console.log(url)
                        /*window.open(url)*/
                        liff.openWindow({
                            url: url
                        });
                        liff.closeWindow();
                        
                    })
                    .catch(function (error) {
                        alert(error);
                        console.log(error)
                    });

            }

        },
        mounted() {
            this.getPDF();
            
        }

    }
</script>
